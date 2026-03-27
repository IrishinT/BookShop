using System;
using System.Collections.Generic;
using System.Linq;

namespace BookShopLibrary
{
    public class Shop
    {
        private List<BookShelf> shelves = new List<BookShelf>();
        public decimal Balance { get; private set; }
        public int MaxShelves { get; private set; }

        public IReadOnlyList<BookShelf> Shelves => shelves.AsReadOnly();
        
        public int ShelfCount => shelves.Count;
        public bool CanAddShelf => shelves.Count < MaxShelves;
        public int TotalBooksCount => shelves.Sum(s => s.CurrentCount);

        public Shop(decimal initialBalance, int maxShelves)
        {
            if (maxShelves <= 0)
                throw new ArgumentException("Максимальное количество шкафов должно быть больше нуля", nameof(maxShelves));
            
            Balance = initialBalance; 
            MaxShelves = maxShelves;
        }

        public void AddToBalance(decimal amount) => Balance += amount;
        
        public List<BookShelf> GetShelves() => shelves.ToList();

        public void AddBookShelf(BookShelf shelf)
        {
            if (shelf == null)
                throw new ArgumentNullException(nameof(shelf));
            
            if (shelves.Count >= MaxShelves)
                throw new InvalidOperationException("Достигнуто максимальное количество шкафов");
            
            shelves.Add(shelf);
        }

        public BookShelf CreateBookShelf(int id, string genre, int capacity)
        {
            var shelf = new BookShelf(id, genre, capacity);
            AddBookShelf(shelf);
            return shelf;
        }

        public bool RemoveBookShelf(int shelfId)
        {
            var shelf = shelves.FirstOrDefault(s => s.Id == shelfId);
            if (shelf == null)
                return false;
            
            if (!shelf.IsEmpty)
                throw new InvalidOperationException("Нельзя удалить непустой шкаф");
            
            return shelves.Remove(shelf);
        }

        public Book? FindBookByTitle(string title)
        {
            foreach (var shelf in shelves)
            {
                var book = shelf.FindBookByTitle(title);
                if (book != null)
                    return book;
            }
            return null;
        }

        public Book? FindBookById(int bookId)
        {
            foreach (var shelf in shelves)
            {
                var book = shelf.FindBookById(bookId);
                if (book != null)
                    return book;
            }
            return null;
        }

        public Dictionary<BookShelf, List<Book>> GetAllBooks()
        {
            var result = new Dictionary<BookShelf, List<Book>>();
            foreach (var shelf in shelves)
            {
                result[shelf] = shelf.GetAllBooks();
            }
            return result;
        }

        public decimal SellBook(int shelfId, int bookId)
        {
            var shelf = shelves.FirstOrDefault(s => s.Id == shelfId);
            if (shelf == null)
                throw new InvalidOperationException("Шкаф не найден");
            
            var price = shelf.SellBook(bookId);
            Balance += price;
            return price;
        }

        // Проверка наличия места (КРИТИЧНО ДЛЯ ОЧЕРЕДИ ПО ТЗ)
        public bool CanFitBook(Book book)
        {
            if (shelves.Any(s => s.Genre == book.Genre && s.HasFreeSpace)) return true;
            if (shelves.Any(s => s.IsEmpty)) return true;
            if (shelves.Count < MaxShelves) return true;
            return false;
        }

        public bool TryAddBook(Book book)
        {
            if (!CanFitBook(book)) return false; // Защита

            // 1. Сиквелы: проверка по текущим книгам на полках (по ТЗ)
            int maxExistingPart = -1;
            foreach (var s in shelves)
            {
                foreach (var b in s.GetAllBooks())
                {
                    if (b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase) && 
                        b.Author.Equals(book.Author, StringComparison.OrdinalIgnoreCase))
                    {
                        if (b.PartNumber > maxExistingPart) maxExistingPart = b.PartNumber;
                    }
                }
            }
            book.PartNumber = maxExistingPart + 1;

            // 2. Добавление в шкаф своего жанра
            var targetShelf = shelves.FirstOrDefault(s => s.Genre == book.Genre && s.HasFreeSpace);
            if (targetShelf != null) return targetShelf.AddBook(book);

            // 3. Смена жанра пустого шкафа (по ТЗ)
            var emptyShelf = shelves.FirstOrDefault(s => s.IsEmpty);
            if (emptyShelf != null)
            {
                emptyShelf.ChangeGenre(book.Genre);
                return emptyShelf.AddBook(book);
            }

            // 4. Создание нового шкафа
            int newId = shelves.Count > 0 ? shelves.Max(s => s.Id) + 1 : 1;
            var newShelf = new BookShelf(newId, book.Genre, 10);
            shelves.Add(newShelf);
            return newShelf.AddBook(book);
        }

        /// <summary>
        /// Прямая продажа из шкафа. По ТЗ: ровно столько, сколько стоила (BaseCost).
        /// </summary>
        public decimal SellBookDirectly(int shelfId, int bookId)
        {
            var shelf = shelves.FirstOrDefault(s => s.Id == shelfId);
            if (shelf == null) throw new InvalidOperationException("Шкаф не найден");

            var book = shelf.FindBookById(bookId);
            if (book == null) throw new InvalidOperationException("Книга не найдена");

            decimal price = book.BaseCost; 
            Balance += price;
            shelf.RemoveBook(bookId);
            return price;
        }

        public void RemoveBookAfterCustomerSale(Book book)
        {
            foreach (var shelf in shelves)
            {
                if (shelf.RemoveBook(book.Id)) break;
            }
        }
    }
}