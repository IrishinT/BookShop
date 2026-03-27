using BookShopLibrary;
using Xunit;

namespace BookShop.Tests
{
    /// <summary>
    /// Класс тестов для проверки функциональности класса GameManager.
    /// Содержит тесты конструктора, игровой логики, условий победы/поражения,
    /// системы заказов, доставок и обслуживания клиентов.
    /// Проверяет основную бизнес-логику игры.
    /// </summary>
    public class GameManagerTests
    {
        #region Конструктор и инициализация

        /// <summary>
        /// Проверяет, что конструктор создаёт GameManager с параметрами средней сложности.
        /// Баланс = 1000, лимит очереди = 4, время = 4 минуты.
        /// </summary>
        [Fact]
        public void Constructor_ShouldCreateGameWithMediumDifficulty()
        {
            // Arrange & Act - Создаём игру со средней сложностью
            var game = new GameManager("Средний");

            // Assert - Проверяем параметры средней сложности
            Assert.Equal(1000m, game.Shop.Balance);      // Начальный баланс
            Assert.Equal(4, game.MaxQueueSize);           // Лимит очереди
            Assert.Equal(4 * 60, game.TimeRemainingSeconds); // 4 минуты
            Assert.Equal(GameState.Playing, game.State);  // Игра идёт
        }

        /// <summary>
        /// Проверяет параметры лёгкой сложности.
        /// Баланс = 2000, лимит очереди = 5, время = 3 минуты.
        /// </summary>
        [Fact]
        public void Constructor_ShouldCreateGameWithEasyDifficulty()
        {
            // Arrange & Act - Создаём игру на лёгком уровне
            var game = new GameManager("Лёгкий");

            // Assert - Проверяем параметры лёгкой сложности
            Assert.Equal(2000m, game.Shop.Balance);
            Assert.Equal(5, game.MaxQueueSize);
            Assert.Equal(3 * 60, game.TimeRemainingSeconds);
        }

        /// <summary>
        /// Проверяет параметры сложной сложности.
        /// Баланс = 500, лимит очереди = 3, время = 5 минут.
        /// </summary>
        [Fact]
        public void Constructor_ShouldCreateGameWithHardDifficulty()
        {
            // Arrange & Act - Создаём игру на сложном уровне
            var game = new GameManager("Сложный");

            // Assert - Проверяем параметры сложной сложности
            Assert.Equal(500m, game.Shop.Balance);
            Assert.Equal(3, game.MaxQueueSize);
            Assert.Equal(5 * 60, game.TimeRemainingSeconds);
        }

        /// <summary>
        /// Проверяет, что при неизвестной сложности используются параметры средней.
        /// </summary>
        [Fact]
        public void Constructor_ShouldUseMediumForUnknownDifficulty()
        {
            // Arrange & Act - Создаём игру с неизвестной сложностью
            var game = new GameManager("Неизвестная");

            // Assert - Используются параметры средней сложности
            Assert.Equal(1000m, game.Shop.Balance);
            Assert.Equal(4, game.MaxQueueSize);
        }

        /// <summary>
        /// Проверяет начальное состояние игры.
        /// </summary>
        [Fact]
        public void Constructor_ShouldInitializeEmptyQueues()
        {
            // Arrange & Act - Создаём игру
            var game = new GameManager("Средний");

            // Assert - Очереди пусты, счётчики обнулены
            Assert.Empty(game.CustomerQueue);
            Assert.Empty(game.DeliveryQueue);
            Assert.Equal(0, game.DissatisfiedCustomers);
            Assert.Equal("", game.EndGameReason);
        }

        /// <summary>
        /// Проверяет, что конструктор инициализирует DatabaseManager.
        /// </summary>
        [Fact]
        public void Constructor_ShouldInitializeDatabaseManager()
        {
            // Arrange & Act - Создаём игру
            var game = new GameManager("Средний");

            // Assert - База данных инициализирована
            Assert.NotEmpty(DatabaseManager.Genres);
            Assert.NotEmpty(DatabaseManager.BookAuthorPairs);
        }

        #endregion

        #region Метод TickSecond

        /// <summary>
        /// Проверяет, что TickSecond уменьшает оставшееся время.
        /// </summary>
        [Fact]
        public void TickSecond_ShouldDecreaseTimeRemaining()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");
            var initialTime = game.TimeRemainingSeconds;

            // Act - Вызываем TickSecond
            game.TickSecond();

            // Assert - Время уменьшилось на 1
            Assert.Equal(initialTime - 1, game.TimeRemainingSeconds);
        }

        /// <summary>
        /// Проверяет, что при достижении времени = 0 игра завершается победой.
        /// </summary>
        [Fact]
        public void TickSecond_ShouldEndGameWithWin_WhenTimeReachesZero()
        {
            // Arrange - Создаём игру и уменьшаем время до 1 секунды
            var game = new GameManager("Средний");
            // Используем рефлексию для установки времени
            var timeField = typeof(GameManager).GetProperty("TimeRemainingSeconds",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            timeField?.SetValue(game, 1);

            // Act - Вызываем TickSecond (время станет 0)
            game.TickSecond();

            // Assert - Игра завершена победой
            Assert.Equal(GameState.Won, game.State);
            Assert.Contains("успешно завершён", game.EndGameReason);
        }

        /// <summary>
        /// Проверяет, что TickSecond не делает ничего после завершения игры.
        /// </summary>
        [Fact]
        public void TickSecond_ShouldDoNothing_AfterGameEnds()
        {
            // Arrange - Создаём игру и завершаем её
            var game = new GameManager("Средний");
            // Завершаем игру через банкротство
            game.Shop.AddToBalance(-2000m);
            game.TickSecond();

            var stateAfterEnd = game.State;

            // Act - Вызываем TickSecond после завершения
            game.TickSecond();

            // Assert - Состояние не изменилось
            Assert.Equal(stateAfterEnd, game.State);
        }

        #endregion

        #region Метод OrderBook

        /// <summary>
        /// Проверяет, что заказ книги списывает средства со счёта.
        /// </summary>
        [Fact]
        public void OrderBook_ShouldDeductMoneyFromBalance()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");

            // Act - Заказываем книгу
            game.OrderBook("Новая книга", "Новый автор", "Фантастика", 300, 500m);

            // Assert - Баланс уменьшился
            Assert.Equal(500m, game.Shop.Balance);
        }

        /// <summary>
        /// Проверяет, что заказ добавляет книгу в очередь доставок.
        /// </summary>
        [Fact]
        public void OrderBook_ShouldAddDeliveryToQueue()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");

            // Act - Заказываем книгу
            game.OrderBook("Книга", "Автор", "Фантастика", 300, 500m);

            // Assert - Доставка в очереди
            Assert.Single(game.DeliveryQueue);
        }

        /// <summary>
        /// Проверяет, что заказная доставка помечена как IsOrdered.
        /// </summary>
        [Fact]
        public void OrderBook_ShouldCreateOrderedDelivery()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");

            // Act - Заказываем книгу
            game.OrderBook("Книга", "Автор", "Фантастика", 300, 500m);

            // Assert - Это заказная доставка
            var delivery = game.DeliveryQueue.Peek();
            Assert.True(delivery.IsOrdered);
            Assert.Equal(DeliveryErrorType.None, delivery.ErrorType);
        }

        /// <summary>
        /// Проверяет, что заказ добавляет новую пару в базу данных.
        /// </summary>
        [Fact]
        public void OrderBook_ShouldAddPairToDatabase()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");
            var initialCount = DatabaseManager.BookAuthorPairs.Count;

            // Act - Заказываем новую книгу
            game.OrderBook("Уникальная книга 12345", "Уникальный автор", "Фантастика", 300, 500m);

            // Assert - Пара добавлена в базу
            Assert.True(DatabaseManager.BookAuthorPairs.Count > initialCount);
        }

        /// <summary>
        /// Проверяет, что при недостатке средств выбрасывается исключение.
        /// </summary>
        [Fact]
        public void OrderBook_ShouldThrowException_WhenInsufficientFunds()
        {
            // Arrange - Создаём игру с малым балансом
            var game = new GameManager("Средний");

            // Act & Assert - Попытка заказа дорогой книги вызывает исключение
            Assert.Throws<Exception>(() => game.OrderBook("Книга", "Автор", "Фантастика", 300, 5000m));
        }

        #endregion

        #region Метод GenerateRandomDelivery

        /// <summary>
        /// Проверяет, что GenerateRandomDelivery добавляет доставку в очередь.
        /// </summary>
        [Fact]
        public void GenerateRandomDelivery_ShouldAddDeliveryToQueue()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");

            // Act - Генерируем случайную доставку
            game.GenerateRandomDelivery();

            // Assert - Доставка в очереди
            Assert.Single(game.DeliveryQueue);
        }

        /// <summary>
        /// Проверяет, что случайная доставка не является заказной.
        /// </summary>
        [Fact]
        public void GenerateRandomDelivery_ShouldCreateNonOrderedDelivery()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");

            // Act - Генерируем доставку
            game.GenerateRandomDelivery();

            // Assert - Не заказная
            var delivery = game.DeliveryQueue.Peek();
            Assert.False(delivery.IsOrdered);
        }

        #endregion

        #region Метод ProcessDelivery

        /// <summary>
        /// Проверяет успешное принятие доставки.
        /// </summary>
        [Fact]
        public void ProcessDelivery_ShouldAcceptDelivery_WhenAccepted()
        {
            // Arrange - Создаём игру и доставку
            var game = new GameManager("Средний");
            game.GenerateRandomDelivery();

            // Act - Принимаем доставку
            var result = game.ProcessDelivery(true, false);

            // Assert - Доставка принята, книга добавлена
            Assert.Empty(game.DeliveryQueue);
            Assert.Equal(1, game.Shop.TotalBooksCount);
            Assert.Contains("размещена", result);
        }

        /// <summary>
        /// Проверяет, что при отсутствии места доставка остаётся в очереди.
        /// </summary>
        [Fact]
        public void ProcessDelivery_ShouldKeepDeliveryInQueue_WhenNoSpace()
        {
            // Arrange - Создаём игру с заполненным магазином
            var game = new GameManager("Средний");
            // Заполняем магазин до отказа
            for (int i = 0; i < 5; i++)
            {
                game.Shop.TryAddBook(new Book(i, $"Книга {i}", "Автор", "Фантастика", 200, 500m));
            }
            game.GenerateRandomDelivery();

            // Act - Пытаемся принять доставку
            var result = game.ProcessDelivery(true, false);

            // Assert - Доставка в очереди, сообщение о предупреждении
            Assert.Single(game.DeliveryQueue);
            Assert.Contains("Нет места", result);
        }

        /// <summary>
        /// Проверяет, что отклонение доставки удаляет её из очереди.
        /// </summary>
        [Fact]
        public void ProcessDelivery_ShouldRemoveFromQueue_WhenRejected()
        {
            // Arrange - Создаём игру и доставку
            var game = new GameManager("Средний");
            game.GenerateRandomDelivery();

            // Act - Отклоняем доставку
            var result = game.ProcessDelivery(false, false);

            // Assert - Доставка удалена из очереди
            Assert.Empty(game.DeliveryQueue);
            Assert.Contains("отклонена", result);
        }

        /// <summary>
        /// Проверяет штраф за принятие бракованной книги.
        /// </summary>
        [Fact]
        public void ProcessDelivery_ShouldPenalizeForAcceptingErrorBook()
        {
            // Arrange - Создаём игру и доставку с ошибкой
            var game = new GameManager("Средний");
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            var delivery = new Delivery(book, false, DeliveryErrorType.Typo);
            game.DeliveryQueue.Enqueue(delivery);
            var initialBalance = game.Shop.Balance;

            // Act - Принимаем бракованную книгу
            var result = game.ProcessDelivery(true, false);

            // Assert - Штраф 15 руб.
            Assert.Equal(initialBalance - 15, game.Shop.Balance);
            Assert.Contains("Штраф", result);
        }

        /// <summary>
        /// Проверяет премию за обнаружение брака.
        /// </summary>
        [Fact]
        public void ProcessDelivery_ShouldRewardForDetectingError()
        {
            // Arrange - Создаём игру и доставку с ошибкой
            var game = new GameManager("Средний");
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            var delivery = new Delivery(book, false, DeliveryErrorType.Plagiarism);
            game.DeliveryQueue.Enqueue(delivery);
            var initialBalance = game.Shop.Balance;

            // Act - Отклоняем как брак
            var result = game.ProcessDelivery(false, true);

            // Assert - Премия 10 руб.
            Assert.Equal(initialBalance + 10, game.Shop.Balance);
            Assert.Contains("премия", result);
        }

        /// <summary>
        /// Проверяет, что деньги за отклонённый заказ не возвращаются.
        /// </summary>
        [Fact]
        public void ProcessDelivery_ShouldNotRefundRejectedOrder()
        {
            // Arrange - Создаём игру и заказываем книгу
            var game = new GameManager("Средний");
            game.OrderBook("Книга", "Автор", "Фантастика", 300, 500m);
            var balanceAfterOrder = game.Shop.Balance;

            // Act - Отклоняем заказную доставку
            var result = game.ProcessDelivery(false, false);

            // Assert - Баланс не изменился (деньги не возвращены)
            Assert.Equal(balanceAfterOrder, game.Shop.Balance);
            Assert.Contains("не возвращены", result);
        }

        /// <summary>
        /// Проверяет сообщение при пустой очереди доставок.
        /// </summary>
        [Fact]
        public void ProcessDelivery_ShouldReturnMessage_WhenQueueIsEmpty()
        {
            // Arrange - Создаём игру без доставок
            var game = new GameManager("Средний");

            // Act - Пытаемся обработать доставку
            var result = game.ProcessDelivery(true, false);

            // Assert - Сообщение о пустой очереди
            Assert.Contains("пуста", result);
        }

        /// <summary>
        /// Проверяет списание денег за случайную книгу при принятии.
        /// </summary>
        [Fact]
        public void ProcessDelivery_ShouldDeductMoneyForRandomBook()
        {
            // Arrange - Создаём игру и случайную доставку
            var game = new GameManager("Средний");
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            var delivery = new Delivery(book, false, DeliveryErrorType.None);
            game.DeliveryQueue.Enqueue(delivery);
            var initialBalance = game.Shop.Balance;

            // Act - Принимаем доставку
            var result = game.ProcessDelivery(true, false);

            // Assert - Деньги списаны
            Assert.Equal(initialBalance - 500m, game.Shop.Balance);
            Assert.Contains("Списано", result);
        }

        #endregion

        #region Метод GenerateCustomer

        /// <summary>
        /// Проверяет, что GenerateCustomer добавляет клиента в очередь.
        /// </summary>
        [Fact]
        public void GenerateCustomer_ShouldAddCustomerToQueue()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");

            // Act - Генерируем клиента
            game.GenerateCustomer();

            // Assert - Клиент в очереди
            Assert.Single(game.CustomerQueue);
        }

        /// <summary>
        /// Проверяет, что GenerateCustomer создаёт валидного клиента.
        /// </summary>
        [Fact]
        public void GenerateCustomer_ShouldCreateValidCustomer()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");

            // Act - Генерируем клиента
            game.GenerateCustomer();

            // Assert - Клиент создан
            var customer = game.CustomerQueue.Peek();
            Assert.NotNull(customer);
        }

        #endregion

        #region Метод ServeCustomer

        /// <summary>
        /// Проверяет успешную продажу книги клиенту.
        /// </summary>
        [Fact]
        public void ServeCustomer_ShouldSucceed_WhenCorrectBookAndPrice()
        {
            // Arrange - Создаём игру, книгу и клиента по жанру
            var game = new GameManager("Средний");
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            game.Shop.TryAddBook(book);

            var customer = new Customer("Фантастика"); // Клиент хочет фантастику
            game.CustomerQueue.Enqueue(customer);
            var initialBalance = game.Shop.Balance;

            // Act - Продаём книгу
            var result = game.ServeCustomer(book, 550m); // Цена в пределах 15% наценки

            // Assert - Продажа успешна
            Assert.Equal(CustomerServiceResult.Success, result);
            Assert.Equal(initialBalance + 550m, game.Shop.Balance);
            Assert.Empty(game.CustomerQueue);
        }

        /// <summary>
        /// Проверяет отказ при завышенной цене (более 15% наценки).
        /// </summary>
        [Fact]
        public void ServeCustomer_ShouldReject_WhenPriceTooHigh()
        {
            // Arrange - Создаём игру, книгу и клиента
            var game = new GameManager("Средний");
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            game.Shop.TryAddBook(book);

            var customer = new Customer("Фантастика");
            game.CustomerQueue.Enqueue(customer);

            // Act - Пытаемся продать с наценкой более 15%
            var result = game.ServeCustomer(book, 600m); // 500 * 1.15 = 575, 600 > 575

            // Assert - Клиент отказался
            Assert.Equal(CustomerServiceResult.PriceTooHigh, result);
            Assert.Equal(1, game.DissatisfiedCustomers);
            Assert.Empty(game.CustomerQueue);
        }

        /// <summary>
        /// Проверяет отказ при предложении не той книги.
        /// </summary>
        [Fact]
        public void ServeCustomer_ShouldReject_WhenWrongBook()
        {
            // Arrange - Создаём игру, книгу и клиента по конкретной книге
            var game = new GameManager("Средний");
            var book = new Book(1, "Другая книга", "Другой автор", "Фантастика", 200, 500m);
            game.Shop.TryAddBook(book);

            var customer = new Customer("Дюна", "Герберт"); // Ищет конкретную книгу
            game.CustomerQueue.Enqueue(customer);

            // Act - Предлагаем не ту книгу
            var result = game.ServeCustomer(book, 500m);

            // Assert - Клиент отказался
            Assert.Equal(CustomerServiceResult.WrongBook, result);
            Assert.Equal(1, game.DissatisfiedCustomers);
        }

        /// <summary>
        /// Проверяет, что при пустой очереди возвращается NoCustomers.
        /// </summary>
        [Fact]
        public void ServeCustomer_ShouldReturnNoCustomers_WhenQueueIsEmpty()
        {
            // Arrange - Создаём игру без клиентов
            var game = new GameManager("Средний");
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            game.Shop.TryAddBook(book);

            // Act - Пытаемся обслужить клиента
            var result = game.ServeCustomer(book, 500m);

            // Assert - Нет клиентов
            Assert.Equal(CustomerServiceResult.NoCustomers, result);
        }

        /// <summary>
        /// Проверяет продажу книги по конкретному запросу (название + автор).
        /// </summary>
        [Fact]
        public void ServeCustomer_ShouldSucceed_WhenExactBookRequested()
        {
            // Arrange - Создаём игру с конкретной книгой
            var game = new GameManager("Средний");
            var book = new Book(1, "Дюна", "Герберт", "Фантастика", 600, 500m);
            game.Shop.TryAddBook(book);

            var customer = new Customer("Дюна", "Герберт"); // Ищет конкретную книгу
            game.CustomerQueue.Enqueue(customer);

            // Act - Продаём правильную книгу
            var result = game.ServeCustomer(book, 500m);

            // Assert - Продажа успешна
            Assert.Equal(CustomerServiceResult.Success, result);
        }

        /// <summary>
        /// Проверяет, что книга удаляется после продажи.
        /// </summary>
        [Fact]
        public void ServeCustomer_ShouldRemoveBook_AfterSale()
        {
            // Arrange - Создаём игру с книгой
            var game = new GameManager("Средний");
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            game.Shop.TryAddBook(book);

            var customer = new Customer("Фантастика");
            game.CustomerQueue.Enqueue(customer);

            // Act - Продаём книгу
            game.ServeCustomer(book, 500m);

            // Assert - Книга удалена из магазина
            Assert.Equal(0, game.Shop.TotalBooksCount);
        }

        #endregion

        #region Метод RejectCustomer

        /// <summary>
        /// Проверяет, что RejectCustomer удаляет клиента и увеличивает счётчик неудовлетворённых.
        /// </summary>
        [Fact]
        public void RejectCustomer_ShouldRemoveCustomerAndIncrementDissatisfied()
        {
            // Arrange - Создаём игру с клиентом
            var game = new GameManager("Средний");
            game.GenerateCustomer();
            var initialDissatisfied = game.DissatisfiedCustomers;

            // Act - Отказываем клиенту
            game.RejectCustomer();

            // Assert - Клиент удалён, счётчик увеличен
            Assert.Empty(game.CustomerQueue);
            Assert.Equal(initialDissatisfied + 1, game.DissatisfiedCustomers);
        }

        /// <summary>
        /// Проверяет, что RejectCustomer не делает ничего при пустой очереди.
        /// </summary>
        [Fact]
        public void RejectCustomer_ShouldDoNothing_WhenQueueIsEmpty()
        {
            // Arrange - Создаём игру без клиентов
            var game = new GameManager("Средний");
            var initialDissatisfied = game.DissatisfiedCustomers;

            // Act - Пытаемся отказать
            game.RejectCustomer();

            // Assert - Счётчик не изменился
            Assert.Equal(initialDissatisfied, game.DissatisfiedCustomers);
        }

        #endregion

        #region Условия поражения

        /// <summary>
        /// Проверяет проигрыш при банкротстве (баланс <= 0).
        /// </summary>
        [Fact]
        public void Game_ShouldEndWithLoss_WhenBankrupt()
        {
            // Arrange - Создаём игру и доводим баланс до 0
            var game = new GameManager("Средний");
            game.Shop.AddToBalance(-1000m); // Баланс = 0

            // Act - Вызываем TickSecond для проверки условий
            game.TickSecond();

            // Assert - Игра проиграна
            Assert.Equal(GameState.Lost, game.State);
            Assert.Contains("Банкротство", game.EndGameReason);
        }

        /// <summary>
        /// Проверяет проигрыш при переполнении очереди.
        /// </summary>
        [Fact]
        public void Game_ShouldEndWithLoss_WhenQueueOverflows()
        {
            // Arrange - Создаём игру и заполняем очередь сверх лимита
            var game = new GameManager("Средний");
            for (int i = 0; i <= game.MaxQueueSize; i++)
            {
                game.GenerateCustomer();
            }

            // Assert - Игра проиграна
            Assert.Equal(GameState.Lost, game.State);
            Assert.Contains("очереди", game.EndGameReason.ToLower());
        }

        /// <summary>
        /// Проверяет проигрыш при слишком многих недовольных клиентах.
        /// </summary>
        [Fact]
        public void Game_ShouldEndWithLoss_WhenTooManyDissatisfied()
        {
            // Arrange - Создаём игру и накапливаем недовольных
            var game = new GameManager("Средний");
            for (int i = 0; i < game.MaxDissatisfied; i++)
            {
                game.GenerateCustomer();
                game.RejectCustomer();
            }

            // Assert - Игра проиграна
            Assert.Equal(GameState.Lost, game.State);
            Assert.Contains("недовольных", game.EndGameReason.ToLower());
        }

        #endregion

        #region Комплексные тесты

        /// <summary>
        /// Комплексный тест: полный цикл заказа, доставки и продажи книги.
        /// </summary>
        [Fact]
        public void Game_ShouldHandleFullCycle()
        {
            // Arrange - Создаём игру
            var game = new GameManager("Средний");
            var initialBalance = game.Shop.Balance;

            // Act 1 - Заказываем книгу
            game.OrderBook("Тестовая книга", "Тестовый автор", "Фантастика", 300, 500m);
            Assert.Equal(initialBalance - 500m, game.Shop.Balance);

            // Act 2 - Принимаем доставку
            var deliveryResult = game.ProcessDelivery(true, false);
            Assert.Contains("доставлен", deliveryResult);
            Assert.Equal(1, game.Shop.TotalBooksCount);

            // Act 3 - Генерируем клиента
            game.GenerateCustomer();
            Assert.Single(game.CustomerQueue);

            // Act 4 - Пробуем продать (если клиенту подходит книга)
            var book = game.Shop.FindBookByTitle("Тестовая книга");
            if (book != null)
            {
                var customer = game.CustomerQueue.Peek();
                bool bookMatches = customer.WantsGenreOnly
                    ? customer.DesiredGenre == book.Genre
                    : customer.DesiredTitle == book.Title && customer.DesiredAuthor == book.Author;

                if (bookMatches)
                {
                    var serveResult = game.ServeCustomer(book, 550m);
                    // Продажа может быть успешной или нет в зависимости от клиента
                }
            }

            // Assert - Игра продолжается
            Assert.Equal(GameState.Playing, game.State);
        }

        /// <summary>
        /// Проверяет, что игра может быть успешно завершена по таймеру.
        /// </summary>
        [Fact]
        public void Game_ShouldEndWithWin_WhenTimeRunsOut()
        {
            // Arrange - Создаём игру и устанавливаем время = 1
            var game = new GameManager("Средний");
            // Устанавливаем время через TickSecond (быстрее)
            while (game.TimeRemainingSeconds > 1)
            {
                game.TickSecond();
            }

            // Act - Последний тик
            game.TickSecond();

            // Assert - Победа
            Assert.Equal(GameState.Won, game.State);
        }

        #endregion

        #region Тесты MaxDissatisfied

        /// <summary>
        /// Проверяет, что MaxDissatisfied = 3 по умолчанию.
        /// </summary>
        [Fact]
        public void MaxDissatisfied_ShouldBeThreeByDefault()
        {
            // Arrange & Act - Создаём игру
            var game = new GameManager("Средний");

            // Assert - Лимит недовольных = 3
            Assert.Equal(3, game.MaxDissatisfied);
        }

        /// <summary>
        /// Проверяет, что при 2 недовольных игра продолжается.
        /// </summary>
        [Fact]
        public void Game_ShouldContinue_WhenTwoDissatisfied()
        {
            // Arrange - Создаём игру и накапливаем 2 недовольных
            var game = new GameManager("Средний");
            game.GenerateCustomer();
            game.RejectCustomer();
            game.GenerateCustomer();
            game.RejectCustomer();

            // Assert - Игра продолжается
            Assert.Equal(GameState.Playing, game.State);
            Assert.Equal(2, game.DissatisfiedCustomers);
        }

        #endregion
    }
}
