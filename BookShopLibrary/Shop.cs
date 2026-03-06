using System;
using System.Collections.Generic;
using System.Linq;
using BookShopLibrary;

namespace BookShopLibrary
{
    /// <summary>
    /// Класс, представляющий книжный магазин.
    /// Управляет шкафами и балансом магазина.
    /// </summary>
    public class Shop
    {
        private List<BookShelf> shelves = new List<BookShelf>();

        /// <summary>
        /// Текущий баланс магазина
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Максимальное количество шкафов в магазине
        /// </summary>
        public int MaxShelves { get; private set; }

        /// <summary>
        /// Конструктор магазина
        /// </summary>
        /// <param name="initialBalance">Начальный баланс</param>
        /// <param name="maxShelves">Максимальное количество шкафов</param>
        public Shop(decimal initialBalance, int maxShelves)
        {
            Balance = initialBalance;
            MaxShelves = maxShelves;
        }

        /// <summary>
        /// Список всех шкафов в магазине
        /// </summary>
        public IReadOnlyList<BookShelf> Shelves => shelves.AsReadOnly();

        /// <summary>
        /// Добавление шкафа в магазин
        /// </summary>
        /// <param name="shelf">Добавляемый шкаф</param>
        /// <exception cref="InvalidOperationException">Если достигнут лимит шкафов</exception>
        public void AddBookShelf(BookShelf shelf)
        {
            if (shelves.Count >= MaxShelves)
                throw new InvalidOperationException("Достигнуто максимальное количество шкафов");

            shelves.Add(shelf);
        }

        /// <summary>
        /// Добавление суммы в баланс магазина
        /// </summary>
        /// <param name="amount">Добавляемая сумма</param>
        public void AddToBalance(decimal amount)
        {
            Balance += amount;
        }

        /// <summary>
        /// Получение всех шкафов магазина
        /// </summary>
        /// <returns>Список шкафов</returns>
        public List<BookShelf> GetShelves()
        {
            return shelves.ToList();
        }
    }
}