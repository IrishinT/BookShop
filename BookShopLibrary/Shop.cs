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
        /// Текущий баланс магазина (заработанные деньги)
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
        /// <param name="maxShelves">Максимальное количество шкафов (n)</param>
        public Shop(decimal initialBalance, int maxShelves)
        {
            if (maxShelves <= 0)
                throw new ArgumentException("Количество шкафов должно быть положительным числом", nameof(maxShelves));

            Balance = initialBalance;
            MaxShelves = maxShelves;
        }

        /// <summary>
        /// Список всех шкафов в магазине (только для чтения)
        /// </summary>
        public IReadOnlyList<BookShelf> Shelves => shelves.AsReadOnly();

        /// <summary>
        /// Текущее количество шкафов в магазине
        /// </summary>
        public int ShelfCount => shelves.Count;

        /// <summary>
        /// Проверка, можно ли добавить новый шкаф
        /// </summary>
        public bool CanAddShelf => shelves.Count < MaxShelves;

        /// <summary>
        /// Общее количество книг в магазине
        /// </summary>
        public int TotalBooksCount => shelves.Sum(s => s.CurrentCount);

        /// <summary>
        /// Добавление шкафа в магазин
        /// </summary>
        /// <param name="shelf">Добавляемый шкаф</param>
        /// <exception cref="InvalidOperationException">Если достигнут лимит шкафов</exception>
        public void AddBookShelf(BookShelf shelf)
        {
            if (shelf == null)
                throw new ArgumentNullException(nameof(shelf));

            if (shelves.Count >= MaxShelves)
                throw new InvalidOperationException($"Достигнуто максимальное количество шкафов ({MaxShelves})");

            shelves.Add(shelf);
        }

        /// <summary>
        /// Создание и добавление нового шкафа
        /// </summary>
        /// <param name="id">ID шкафа</param>
        /// <param name="genre">Жанр</param>
        /// <param name="capacity">Вместимость</param>
        /// <returns>Созданный шкаф</returns>
        public BookShelf CreateBookShelf(int id, string genre, int capacity)
        {
            var newShelf = new BookShelf(id, genre, capacity);
            AddBookShelf(newShelf);
            return newShelf;
        }

        /// <summary>
        /// Удаление шкафа из магазина (только пустой)
        /// </summary>
        /// <param name="shelfId">ID шкафа</param>
        /// <returns>true - шкаф удален, false - шкаф не найден</returns>
        /// <exception cref="InvalidOperationException">Если шкаф не пуст</exception>
        public bool RemoveBookShelf(int shelfId)
        {
            var shelf = shelves.FirstOrDefault(s => s.Id == shelfId);
            if (shelf == null)
                return false;

            if (!shelf.IsEmpty)
                throw new InvalidOperationException("Нельзя удалить непустой шкаф");

            return shelves.Remove(shelf);
        }

        /// <summary>
        /// Добавление суммы в баланс магазина
        /// </summary>
        /// <param name="amount">Добавляемая сумма</param>
        public void AddToBalance(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Сумма не может быть отрицательной", nameof(amount));

            Balance += amount;
        }

        /// <summary>
        /// Поиск книги по названию во всех шкафах
        /// </summary>
        /// <param name="title">Название книги</param>
        /// <returns>Найденная книга или null</returns>
        public Book FindBookByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return null;

            foreach (var shelf in shelves)
            {
                var book = shelf.FindBookByTitle(title);
                if (book != null)
                    return book;
            }
            return null;
        }

        /// <summary>
        /// Поиск книги по ID во всех шкафах
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <returns>Найденная книга или null</returns>
        public Book FindBookById(int id)
        {
            foreach (var shelf in shelves)
            {
                var book = shelf.FindBookById(id);
                if (book != null)
                    return book;
            }
            return null;
        }

        /// <summary>
        /// Получение всех книг в магазине
        /// </summary>
        /// <returns>Словарь: ключ - шкаф, значение - список книг</returns>
        public Dictionary<BookShelf, List<Book>> GetAllBooks()
        {
            var result = new Dictionary<BookShelf, List<Book>>();
            foreach (var shelf in shelves)
            {
                result[shelf] = shelf.GetAllBooks();
            }
            return result;
        }

        /// <summary>
        /// Получение всех шкафов магазина
        /// </summary>
        /// <returns>Список шкафов</returns>
        public List<BookShelf> GetShelves()
        {
            return shelves.ToList();
        }

        /// <summary>
        /// Попытка добавить книгу в магазин с автоматическим подбором шкафа
        /// </summary>
        /// <param name="book">Добавляемая книга</param>
        /// <returns>true - книга добавлена, false - нет места</returns>
        public bool TryAddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            // 1. Поиск шкафа с подходящим жанром и свободным местом
            var targetShelf = shelves.FirstOrDefault(s =>
                s.Genre == book.Genre && s.HasFreeSpace);

            if (targetShelf != null)
            {
                return targetShelf.AddBook(book);
            }

            // 2. Поиск пустого шкафа для переназначения жанра
            var emptyShelf = shelves.FirstOrDefault(s => s.IsEmpty);
            if (emptyShelf != null)
            {
                emptyShelf.ChangeGenre(book.Genre);
                return emptyShelf.AddBook(book);
            }

            // 3. Создание нового шкафа, если есть место
            if (shelves.Count < MaxShelves)
            {
                int newId = shelves.Count > 0 ? shelves.Max(s => s.Id) + 1 : 1;
                var newShelf = CreateBookShelf(newId, book.Genre, 10);
                return newShelf.AddBook(book);
            }

            return false;
        }
    }
}