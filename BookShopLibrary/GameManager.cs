using System;
using System.Collections.Generic;

namespace BookShopLibrary
{
    public class GameManager
    {
        public Shop Shop { get; private set; }
        public Queue<Customer> CustomerQueue { get; private set; } = new Queue<Customer>();
        public Queue<Delivery> DeliveryQueue { get; private set; } = new Queue<Delivery>();

        public GameState State { get; private set; } = GameState.Playing;
        public string EndGameReason { get; private set; } = "";

        public int MaxQueueSize { get; private set; }
        
        // Счётчик неудовлетворённых клиентов (По ТЗ)
        public int DissatisfiedCustomers { get; private set; } 
        public int MaxDissatisfied { get; private set; } = 3;

        public int TimeRemainingSeconds { get; private set; }
        private int nextBookId = 1;

        public GameManager(string difficulty)
        {
            DatabaseManager.Initialize();

            decimal startBalance;
            int gameDurationMins;

            switch (difficulty)
            {
                case "Сложный": startBalance = 500; MaxQueueSize = 3; gameDurationMins = 5; break;
                case "Лёгкий": startBalance = 2000; MaxQueueSize = 5; gameDurationMins = 3; break;
                default: startBalance = 1000; MaxQueueSize = 4; gameDurationMins = 4; break;
            }

            Shop = new Shop(startBalance, 5);
            TimeRemainingSeconds = gameDurationMins * 60;
        }

        public void TickSecond()
        {
            if (State != GameState.Playing) return;
            TimeRemainingSeconds--;
            CheckLossConditions();
            if (TimeRemainingSeconds <= 0 && State == GameState.Playing)
                EndGame(true, "Рабочий день успешно завершён!");
        }

        /// <summary> Заказ книги. По ТЗ деньги списываются сразу при нажатии кнопки. </summary>
        public void OrderBook(string title, string author, string genre, int pages, decimal baseCost)
        {
            if (Shop.Balance < baseCost) throw new Exception("Недостаточно средств для заказа!");

            Shop.AddToBalance(-baseCost);
            DatabaseManager.AddNewPair(title, author);

            Book newBook = new Book(nextBookId++, title, author, genre, pages, baseCost);
            DeliveryQueue.Enqueue(new Delivery(newBook, true, DeliveryErrorType.None));
        }

        public void GenerateRandomDelivery() => DeliveryQueue.Enqueue(Delivery.GenerateRandomDelivery(nextBookId++));

        /// <summary> Обработка поставки </summary>
        public string ProcessDelivery(bool accept, bool markAsError)
        {
            if (DeliveryQueue.Count == 0) return "Очередь поставок пуста.";
            
            // ВАЖНО: Смотрим на книгу (Peek), но пока НЕ ИЗВЛЕКАЕМ!
            var delivery = DeliveryQueue.Peek();
            string resultMsg = "";
            bool hasRealError = delivery.ErrorType != DeliveryErrorType.None;

            if (accept)
            {
                // По ТЗ: "Если под книгу нет места... она остаётся в очереди, а пользователю демонстрируется предупреждение"
                if (!Shop.CanFitBook(delivery.Book))
                {
                    return "ВНИМАНИЕ: Нет места на полках! Поставка осталась в очереди.";
                }

                // Место есть, извлекаем!
                DeliveryQueue.Dequeue();

                if (hasRealError)
                {
                    Shop.AddToBalance(-15); // Штраф за невнимательность
                    resultMsg = "Вы приняли бракованную книгу! Штраф 15 руб.\n";
                }
                else
                {
                    if (!delivery.IsOrdered) 
                    {
                        Shop.AddToBalance(-delivery.Book.BaseCost); // Списываем деньги за случайную книгу
                        resultMsg = $"Книга принята. Списано {delivery.Book.BaseCost:C}.\n";
                    }
                    else resultMsg = "Заказ успешно доставлен.\n";
                }

                Shop.TryAddBook(delivery.Book);
                resultMsg += "Книга размещена на полке.";
            }
            else // Отклоняем
            {
                DeliveryQueue.Dequeue(); // Извлекаем из очереди и выкидываем

                if (hasRealError && markAsError)
                {
                    Shop.AddToBalance(10); // Премия
                    resultMsg = "Вы вовремя выявили брак! Выдана премия 10 руб.";
                }
                else if (!hasRealError && markAsError)
                    resultMsg = "Ошибок не было, но вы отклонили хорошую книгу.";
                else
                    resultMsg = "Книга отклонена.";

                // По ТЗ: деньги за отклоненный ЗАКАЗ не возвращаются
                if (delivery.IsOrdered) resultMsg += " Средства за заказ не возвращены!";
            }

            CheckLossConditions();
            return resultMsg;
        }

        public void GenerateCustomer()
        {
            CustomerQueue.Enqueue(Customer.GenerateRandomCustomer());
            CheckLossConditions();
        }

        /// <summary>
        /// Попытка продать книгу клиенту. Контролирует наценку (не более 15%).
        /// </summary>
        public CustomerServiceResult ServeCustomer(Book bookOffered, decimal requestedPrice)
        {
            if (CustomerQueue.Count == 0) return CustomerServiceResult.NoCustomers;
            Customer currentCustomer = CustomerQueue.Peek();

            // 1. Проверка наценки (макс +15% от BaseCost)
            decimal maxAllowedPrice = bookOffered.BaseCost * 1.15m; 
            if (requestedPrice > maxAllowedPrice)
            {
                DissatisfiedCustomers++; // По ТЗ: счетчик неудовлетворенных растет
                CustomerQueue.Dequeue();
                CheckLossConditions();
                return CustomerServiceResult.PriceTooHigh;
            }

            // 2. Соответствие запросу (конкретная или жанр)
            bool isCorrectBook = currentCustomer.WantsGenreOnly 
                ? (bookOffered.Genre == currentCustomer.DesiredGenre)
                : (bookOffered.Title.Equals(currentCustomer.DesiredTitle, StringComparison.OrdinalIgnoreCase) && 
                   bookOffered.Author.Equals(currentCustomer.DesiredAuthor, StringComparison.OrdinalIgnoreCase));

            if (!isCorrectBook)
            {
                DissatisfiedCustomers++;
                CustomerQueue.Dequeue();
                CheckLossConditions();
                return CustomerServiceResult.WrongBook;
            }

            // 3. Успех
            Shop.AddToBalance(requestedPrice);
            Shop.RemoveBookAfterCustomerSale(bookOffered);
            CustomerQueue.Dequeue();
            return CustomerServiceResult.Success;
        }

        /// <summary> Прямой отказ клиенту (например, книги нет и заказать не хотим) </summary>
        public void RejectCustomer()
        {
            if (CustomerQueue.Count > 0)
            {
                CustomerQueue.Dequeue();
                DissatisfiedCustomers++; // Прямой отказ
                CheckLossConditions();
            }
        }

        private void CheckLossConditions()
        {
            if (State != GameState.Playing) return;

            if (Shop.Balance <= 0) 
                EndGame(false, "Банкротство! Баланс упал до нуля.");
            else if (CustomerQueue.Count > MaxQueueSize) 
                EndGame(false, $"Очередь превысила лимит ({MaxQueueSize} чел).");
            else if (DissatisfiedCustomers >= MaxDissatisfied) 
                EndGame(false, $"Слишком много недовольных клиентов ({MaxDissatisfied}/{MaxDissatisfied}).");
        }

        private void EndGame(bool isWin, string reason)
        {
            State = isWin ? GameState.Won : GameState.Lost;
            EndGameReason = reason;
            DatabaseManager.LogGameResult(isWin, Shop.Balance, reason);
        }
    }
}