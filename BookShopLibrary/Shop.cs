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
        /// Словарь для отслеживания максимальных номеров частей книг (сиквелов)
        /// Ключ: "Название|Автор" (уникальная комбинация)
        /// Значение: максимальный номер части для этой серии
        /// </summary>
        private Dictionary<string, int> _maxPartNumbers = new Dictionary<string, int>();

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
        /// Реализует логику:
        /// 1. Определяет, является ли книга сиквелом (продолжением)
        /// 2. Ищет шкаф с таким же жанром и свободным местом
        /// 3. Если нет подходящего, ищет пустой шкаф и меняет его жанр
        /// 4. Если нет пустого, создаёт новый шкаф (если есть место)
        /// </summary>
        /// <param name="book">Добавляемая книга</param>
        /// <returns>true - книга добавлена, false - нет места</returns>
        public bool TryAddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            // ========== 1. Обработка сиквелов ==========
            string bookKey = $"{book.Title}|{book.Author}";

            if (_maxPartNumbers.ContainsKey(bookKey))
            {
                // Книга этой серии уже есть - это сиквел (продолжение)
                int nextPart = _maxPartNumbers[bookKey] + 1;
                book.PartNumber = nextPart;
                _maxPartNumbers[bookKey] = nextPart;
            }
            else
            {
                // Новая серия книг
                _maxPartNumbers[bookKey] = 0;
            }

            // ========== 2. Поиск шкафа с подходящим жанром ==========
            var targetShelf = shelves.FirstOrDefault(s =>
                s.Genre == book.Genre && s.HasFreeSpace);

            if (targetShelf != null)
            {
                return targetShelf.AddBook(book);
            }

            // ========== 3. Поиск пустого шкафа для переназначения жанра ==========
            var emptyShelf = shelves.FirstOrDefault(s => s.IsEmpty);
            if (emptyShelf != null)
            {
                emptyShelf.ChangeGenre(book.Genre);
                return emptyShelf.AddBook(book);
            }

            // ========== 4. Создание нового шкафа, если есть место ==========
            if (shelves.Count < MaxShelves)
            {
                int newId = shelves.Count > 0 ? shelves.Max(s => s.Id) + 1 : 1;
                var newShelf = CreateBookShelf(newId, book.Genre, 10);
                return newShelf.AddBook(book);
            }

            return false;
        }

        /// <summary>
        /// Продаёт книгу по ID шкафа и ID книги
        /// Обновляет словарь частей и баланс магазина
        /// </summary>
        /// <param name="shelfId">ID шкафа, в котором находится книга</param>
        /// <param name="bookId">ID книги для продажи</param>
        /// <returns>Цена проданной книги</returns>
        /// <exception cref="InvalidOperationException">Если шкаф или книга не найдены</exception>
        public decimal SellBook(int shelfId, int bookId)
        {
            // Находим нужный шкаф
            var shelf = shelves.FirstOrDefault(s => s.Id == shelfId);
            if (shelf == null)
                throw new InvalidOperationException("Шкаф не найден");

            // Находим нужную книгу
            var book = shelf.FindBookById(bookId);
            if (book == null)
                throw new InvalidOperationException("Книга не найдена");

            // Получаем цену и добавляем её на баланс
            decimal price = book.Sell();
            Balance += price;

            // ========== Обновление словаря частей ==========
            // Проверяем, не была ли это последняя книга определённой части
            string bookKey = $"{book.Title}|{book.Author}";
            if (_maxPartNumbers.ContainsKey(bookKey))
            {
                // Проверяем, остались ли ещё книги этой серии в магазине
                bool hasOtherParts = shelves.Any(s => s.GetAllBooks().Any(b =>
                    b.Title == book.Title && b.Author == book.Author && b.Id != bookId));

                if (!hasOtherParts)
                {
                    // Если не осталось ни одной книги этой серии, удаляем из словаря
                    // Это нужно, чтобы при повторном поступлении книги она стала оригиналом,
                    // а не продолжала нумерацию с проданного места
                    _maxPartNumbers.Remove(bookKey);
                }
            }

            // Удаляем книгу из шкафа
            shelf.RemoveBook(bookId);
            return price;
        }

        /// <summary>
        /// Проверяет уникальность названия книги и добавляет числовой индекс при необходимости
        /// (используется для старых методов, не использующих сиквелы)
        /// </summary>
        /// <param name="book">Книга для проверки</param>
        private void EnsureUniqueTitle(Book book)
        {
            string originalTitle = book.Title;
            int index = 2;

            // Проверяем, существует ли книга с таким названием
            while (FindBookByTitle(book.Title) != null)
            {
                // Если название уже существует, добавляем индекс
                book.Title = $"{originalTitle} {index}";
                index++;
            }
        }
    }
}
