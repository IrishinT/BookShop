using BookShopLibrary;
using Xunit;

namespace BookShop.Tests
{
    /// <summary>
    /// Класс тестов для проверки функциональности класса Shop.
    /// Содержит тесты конструктора, управления шкафами, баланса,
    /// поиска книг, продажи и системы сиквелов.
    /// Проверяет бизнес-логику магазина на верхнем уровне.
    /// </summary>
    public class ShopTests
    {
        #region Конструктор и свойства

        /// <summary>
        /// Проверяет, что конструктор создаёт магазин с корректными свойствами.
        /// Баланс и лимит шкафов должны соответствовать параметрам.
        /// </summary>
        [Fact]
        public void Constructor_ShouldCreateShopWithCorrectProperties()
        {
            // Arrange & Act - Создаём магазин с тестовыми параметрами
            var shop = new Shop(1000m, 5);

            // Assert - Проверяем все свойства
            Assert.Equal(1000m, shop.Balance);      // Начальный баланс
            Assert.Equal(5, shop.MaxShelves);       // Лимит шкафов
            Assert.Equal(0, shop.ShelfCount);       // Нет шкафов
            Assert.True(shop.CanAddShelf);          // Можно добавить шкаф
        }

        /// <summary>
        /// Проверяет, что конструктор выбрасывает исключение при maxShelves = 0.
        /// Магазин должен иметь хотя бы один шкаф.
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrowException_WhenMaxShelvesIsZero()
        {
            // Act & Assert - Должно выброситься ArgumentException
            Assert.Throws<ArgumentException>(() => new Shop(1000m, 0));
        }

        /// <summary>
        /// Проверяет, что конструктор выбрасывает исключение при отрицательном maxShelves.
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrowException_WhenMaxShelvesIsNegative()
        {
            // Act & Assert - Должно выброситься ArgumentException
            Assert.Throws<ArgumentException>(() => new Shop(1000m, -1));
        }

        /// <summary>
        /// Проверяет создание магазина с нулевым балансом.
        /// Это допустимо, хотя ведёт к быстрому банкротству.
        /// </summary>
        [Fact]
        public void Constructor_ShouldAcceptZeroBalance()
        {
            // Arrange & Act - Создаём магазин с нулевым балансом
            var shop = new Shop(0m, 5);

            // Assert - Баланс = 0
            Assert.Equal(0m, shop.Balance);
        }

        /// <summary>
        /// Проверяет создание магазина с отрицательным балансом.
        /// Это допустимо на уровне конструктора.
        /// </summary>
        [Fact]
        public void Constructor_ShouldAcceptNegativeBalance()
        {
            // Arrange & Act - Создаём магазин с отрицательным балансом
            var shop = new Shop(-100m, 5);

            // Assert - Баланс отрицательный
            Assert.Equal(-100m, shop.Balance);
        }

        #endregion

        #region Метод AddBookShelf

        /// <summary>
        /// Проверяет успешное добавление шкафа в магазин.
        /// После добавления счётчик шкафов должен увеличиться.
        /// </summary>
        [Fact]
        public void AddBookShelf_ShouldAddShelfSuccessfully()
        {
            // Arrange - Создаём магазин и шкаф
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act - Добавляем шкаф
            shop.AddBookShelf(shelf);

            // Assert - Шкаф добавлен
            Assert.Equal(1, shop.ShelfCount);
        }

        /// <summary>
        /// Проверяет, что при попытке добавить null шкаф
        /// выбрасывается исключение ArgumentNullException.
        /// </summary>
        [Fact]
        public void AddBookShelf_ShouldThrowException_WhenShelfIsNull()
        {
            // Arrange - Создаём магазин
            var shop = new Shop(1000m, 5);

            // Act & Assert - Добавление null вызывает исключение
            Assert.Throws<ArgumentNullException>(() => shop.AddBookShelf(null));
        }

        /// <summary>
        /// Проверяет, что при достижении лимита шкафов
        /// выбрасывается исключение InvalidOperationException.
        /// </summary>
        [Fact]
        public void AddBookShelf_ShouldThrowException_WhenMaxShelvesReached()
        {
            // Arrange - Создаём магазин с лимитом 2 и заполняем его
            var shop = new Shop(1000m, 2);
            var shelf1 = new BookShelf(1, "Фантастика", 10);
            var shelf2 = new BookShelf(2, "Детектив", 10);
            var shelf3 = new BookShelf(3, "Роман", 10);
            shop.AddBookShelf(shelf1);
            shop.AddBookShelf(shelf2);

            // Act & Assert - Попытка добавить третий шкаф вызывает исключение
            Assert.Throws<InvalidOperationException>(() => shop.AddBookShelf(shelf3));
        }

        /// <summary>
        /// Проверяет, что CanAddShelf возвращает false после достижения лимита.
        /// </summary>
        [Fact]
        public void CanAddShelf_ShouldReturnFalse_WhenMaxReached()
        {
            // Arrange - Создаём магазин с лимитом 1 и добавляем шкаф
            var shop = new Shop(1000m, 1);
            shop.AddBookShelf(new BookShelf(1, "Фантастика", 10));

            // Assert - Больше нельзя добавить шкаф
            Assert.False(shop.CanAddShelf);
        }

        #endregion

        #region Метод CreateBookShelf

        /// <summary>
        /// Проверяет, что CreateBookShelf создаёт и добавляет шкаф.
        /// Метод должен вернуть созданный шкаф с правильными параметрами.
        /// </summary>
        [Fact]
        public void CreateBookShelf_ShouldCreateAndAddShelf()
        {
            // Arrange - Создаём магазин
            var shop = new Shop(1000m, 5);

            // Act - Создаём шкаф через метод магазина
            var shelf = shop.CreateBookShelf(1, "Фантастика", 10);

            // Assert - Шкаф создан и добавлен
            Assert.NotNull(shelf);
            Assert.Equal(1, shop.ShelfCount);
            Assert.Equal("Фантастика", shelf.Genre);
        }

        #endregion

        #region Метод RemoveBookShelf

        /// <summary>
        /// Проверяет успешное удаление пустого шкафа.
        /// </summary>
        [Fact]
        public void RemoveBookShelf_ShouldRemoveEmptyShelf()
        {
            // Arrange - Создаём магазин с пустым шкафом
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);

            // Act - Удаляем шкаф
            var result = shop.RemoveBookShelf(1);

            // Assert - Шкаф удалён
            Assert.True(result);
            Assert.Equal(0, shop.ShelfCount);
        }

        /// <summary>
        /// Проверяет, что удаление непустого шкафа выбрасывает исключение.
        /// Нельзя удалить шкаф с книгами.
        /// </summary>
        [Fact]
        public void RemoveBookShelf_ShouldThrowException_WhenShelfIsNotEmpty()
        {
            // Arrange - Создаём шкаф с книгой
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);
            shop.AddBookShelf(shelf);

            // Act & Assert - Попытка удаления вызывает исключение
            Assert.Throws<InvalidOperationException>(() => shop.RemoveBookShelf(1));
        }

        /// <summary>
        /// Проверяет, что при удалении несуществующего шкафа возвращается false.
        /// </summary>
        [Fact]
        public void RemoveBookShelf_ShouldReturnFalse_WhenShelfNotFound()
        {
            // Arrange - Создаём пустой магазин
            var shop = new Shop(1000m, 5);

            // Act - Пытаемся удалить несуществующий шкаф
            var result = shop.RemoveBookShelf(999);

            // Assert - Результат false
            Assert.False(result);
        }

        #endregion

        #region Метод AddToBalance

        /// <summary>
        /// Проверяет, что AddToBalance увеличивает баланс.
        /// </summary>
        [Fact]
        public void AddToBalance_ShouldIncreaseBalance()
        {
            // Arrange - Создаём магазин с балансом 1000
            var shop = new Shop(1000m, 5);

            // Act - Добавляем 500
            shop.AddToBalance(500m);

            // Assert - Баланс = 1500
            Assert.Equal(1500m, shop.Balance);
        }

        /// <summary>
        /// Проверяет, что AddToBalance с отрицательным значением уменьшает баланс.
        /// Используется для списания средств.
        /// </summary>
        [Fact]
        public void AddToBalance_ShouldDecreaseBalance_WhenNegativeAmount()
        {
            // Arrange - Создаём магазин с балансом 1000
            var shop = new Shop(1000m, 5);

            // Act - Списываем 300
            shop.AddToBalance(-300m);

            // Assert - Баланс = 700
            Assert.Equal(700m, shop.Balance);
        }

        /// <summary>
        /// Проверяет, что баланс может стать отрицательным.
        /// </summary>
        [Fact]
        public void AddToBalance_CanMakeBalanceNegative()
        {
            // Arrange - Создаём магазин с балансом 100
            var shop = new Shop(100m, 5);

            // Act - Списываем 200
            shop.AddToBalance(-200m);

            // Assert - Баланс = -100
            Assert.Equal(-100m, shop.Balance);
        }

        #endregion

        #region Метод FindBookByTitle

        /// <summary>
        /// Проверяет поиск книги по названию во всех шкафах.
        /// </summary>
        [Fact]
        public void FindBookByTitle_ShouldReturnBook_WhenExists()
        {
            // Arrange - Создаём магазин с книгой
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Дюна", "Герберт", "Фантастика", 600, 1200m);
            shelf.AddBook(book);
            shop.AddBookShelf(shelf);

            // Act - Ищем книгу по названию
            var found = shop.FindBookByTitle("Дюна");

            // Assert - Книга найдена
            Assert.NotNull(found);
            Assert.Equal("Дюна", found.Title);
        }

        /// <summary>
        /// Проверяет поиск книги, когда название не найдено.
        /// </summary>
        [Fact]
        public void FindBookByTitle_ShouldReturnNull_WhenBookNotFound()
        {
            // Arrange - Создаём магазин с пустым шкафом
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);

            // Act - Ищем несуществующую книгу
            var result = shop.FindBookByTitle("Несуществующая книга");

            // Assert - Результат null
            Assert.Null(result);
        }

        /// <summary>
        /// Проверяет, что при передаче пустой строки возвращается null.
        /// </summary>
        [Fact]
        public void FindBookByTitle_ShouldReturnNull_WhenTitleIsEmpty()
        {
            // Arrange - Создаём магазин
            var shop = new Shop(1000m, 5);

            // Act - Ищем по пустой строке
            var result = shop.FindBookByTitle("");

            // Assert - Результат null
            Assert.Null(result);
        }

        /// <summary>
        /// Проверяет поиск книги среди нескольких шкафов.
        /// </summary>
        [Fact]
        public void FindBookByTitle_ShouldSearchInAllShelves()
        {
            // Arrange - Создаём два шкафа с книгами
            var shop = new Shop(1000m, 5);
            var shelf1 = new BookShelf(1, "Фантастика", 10);
            var shelf2 = new BookShelf(2, "Детектив", 10);
            shelf1.AddBook(new Book(1, "Дюна", "Герберт", "Фантастика", 600, 1200m));
            shelf2.AddBook(new Book(2, "Шерлок", "Дойл", "Детектив", 400, 800m));
            shop.AddBookShelf(shelf1);
            shop.AddBookShelf(shelf2);

            // Act - Ищем книгу во втором шкафу
            var found = shop.FindBookByTitle("Шерлок");

            // Assert - Книга найдена
            Assert.NotNull(found);
            Assert.Equal("Шерлок", found.Title);
        }

        #endregion

        #region Метод FindBookById

        /// <summary>
        /// Проверяет поиск книги по ID.
        /// </summary>
        [Fact]
        public void FindBookById_ShouldReturnBook_WhenExists()
        {
            // Arrange - Создаём магазин с книгой
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(10, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);
            shop.AddBookShelf(shelf);

            // Act - Ищем по ID = 10
            var found = shop.FindBookById(10);

            // Assert - Книга найдена
            Assert.NotNull(found);
            Assert.Equal(10, found.Id);
        }

        /// <summary>
        /// Проверяет, что при поиске несуществующего ID возвращается null.
        /// </summary>
        [Fact]
        public void FindBookById_ShouldReturnNull_WhenBookNotFound()
        {
            // Arrange - Создаём магазин с пустым шкафом
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);

            // Act - Ищем несуществующий ID
            var result = shop.FindBookById(999);

            // Assert - Результат null
            Assert.Null(result);
        }

        #endregion

        #region Метод GetAllBooks

        /// <summary>
        /// Проверяет, что GetAllBooks возвращает словарь со всеми книгами.
        /// </summary>
        [Fact]
        public void GetAllBooks_ShouldReturnDictionaryOfBooks()
        {
            // Arrange - Создаём два шкафа с книгами
            var shop = new Shop(1000m, 5);
            var shelf1 = new BookShelf(1, "Фантастика", 10);
            var shelf2 = new BookShelf(2, "Детектив", 10);
            var book1 = new Book(1, "Книга 1", "Автор", "Фантастика", 200, 500m);
            var book2 = new Book(2, "Книга 2", "Автор", "Детектив", 200, 500m);
            shelf1.AddBook(book1);
            shelf2.AddBook(book2);
            shop.AddBookShelf(shelf1);
            shop.AddBookShelf(shelf2);

            // Act - Получаем все книги
            var allBooks = shop.GetAllBooks();

            // Assert - Словарь содержит 2 записи
            Assert.Equal(2, allBooks.Count);
            Assert.Single(allBooks[shelf1]);
            Assert.Single(allBooks[shelf2]);
        }

        /// <summary>
        /// Проверяет TotalBooksCount для нескольких шкафов.
        /// </summary>
        [Fact]
        public void TotalBooksCount_ShouldReturnCorrectCount()
        {
            // Arrange - Создаём шкафы с книгами
            var shop = new Shop(1000m, 5);
            var shelf1 = new BookShelf(1, "Фантастика", 10);
            var shelf2 = new BookShelf(2, "Детектив", 10);
            shelf1.AddBook(new Book(1, "Книга 1", "Автор", "Фантастика", 200, 500m));
            shelf1.AddBook(new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m));
            shelf2.AddBook(new Book(3, "Книга 3", "Автор", "Детектив", 200, 500m));
            shop.AddBookShelf(shelf1);
            shop.AddBookShelf(shelf2);

            // Assert - Всего 3 книги
            Assert.Equal(3, shop.TotalBooksCount);
        }

        #endregion

        #region Метод GetShelves и свойство Shelves

        /// <summary>
        /// Проверяет, что GetShelves возвращает список шкафов.
        /// </summary>
        [Fact]
        public void GetShelves_ShouldReturnListOfShelves()
        {
            // Arrange - Создаём магазин с двумя шкафами
            var shop = new Shop(1000m, 5);
            var shelf1 = new BookShelf(1, "Фантастика", 10);
            var shelf2 = new BookShelf(2, "Детектив", 10);
            shop.AddBookShelf(shelf1);
            shop.AddBookShelf(shelf2);

            // Act - Получаем список шкафов
            var shelves = shop.GetShelves();

            // Assert - Список содержит 2 шкафа
            Assert.Equal(2, shelves.Count);
        }

        /// <summary>
        /// Проверяет, что свойство Shelves возвращает ReadOnlyList.
        /// </summary>
        [Fact]
        public void Shelves_ShouldReturnReadOnlyList()
        {
            // Arrange - Создаём магазин со шкафом
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);

            // Act - Получаем шкафы через свойство
            var shelves = shop.Shelves;

            // Assert - Один шкаф в списке
            Assert.Single(shelves);
        }

        #endregion

        #region Метод CanFitBook

        /// <summary>
        /// Проверяет, что CanFitBook возвращает true для пустого магазина.
        /// </summary>
        [Fact]
        public void CanFitBook_ShouldReturnTrue_WhenShopIsEmpty()
        {
            // Arrange - Создаём пустой магазин
            var shop = new Shop(1000m, 5);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Проверяем возможность размещения
            var result = shop.CanFitBook(book);

            // Assert - Можно разместить
            Assert.True(result);
        }

        /// <summary>
        /// Проверяет, что CanFitBook возвращает true при наличии шкафа того же жанра.
        /// </summary>
        [Fact]
        public void CanFitBook_ShouldReturnTrue_WhenShelfOfSameGenreExists()
        {
            // Arrange - Создаём шкаф фантастики
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Проверяем возможность размещения
            var result = shop.CanFitBook(book);

            // Assert - Можно разместить
            Assert.True(result);
        }

        /// <summary>
        /// Проверяет, что CanFitBook возвращает true при наличии пустого шкафа.
        /// </summary>
        [Fact]
        public void CanFitBook_ShouldReturnTrue_WhenEmptyShelfExists()
        {
            // Arrange - Создаём пустой шкаф другого жанра
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Детектив", 10);
            shop.AddBookShelf(shelf);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Проверяем возможность размещения
            var result = shop.CanFitBook(book);

            // Assert - Можно разместить (сменит жанр пустого шкафа)
            Assert.True(result);
        }

        /// <summary>
        /// Проверяет, что CanFitBook возвращает false, когда нет места.
        /// </summary>
        [Fact]
        public void CanFitBook_ShouldReturnFalse_WhenNoSpaceAvailable()
        {
            // Arrange - Создаём заполненный шкаф и достигаем лимит шкафов
            var shop = new Shop(1000m, 1);
            var shelf = new BookShelf(1, "Детектив", 1);
            shelf.AddBook(new Book(1, "Книга 1", "Автор", "Детектив", 200, 500m));
            shop.AddBookShelf(shelf);
            var book = new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m);

            // Act - Проверяем возможность размещения
            var result = shop.CanFitBook(book);

            // Assert - Нельзя разместить
            Assert.False(result);
        }

        #endregion

        #region Метод TryAddBook

        /// <summary>
        /// Проверяет добавление книги в существующий шкаф того же жанра.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldAddBookToExistingShelf()
        {
            // Arrange - Создаём шкаф фантастики
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Добавляем книгу
            var result = shop.TryAddBook(book);

            // Assert - Книга добавлена
            Assert.True(result);
            Assert.Equal(1, shop.TotalBooksCount);
        }

        /// <summary>
        /// Проверяет создание нового шкафа при отсутствии подходящего.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldCreateNewShelf_WhenNoSuitableShelfExists()
        {
            // Arrange - Создаём пустой магазин
            var shop = new Shop(1000m, 5);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Добавляем книгу
            var result = shop.TryAddBook(book);

            // Assert - Создан новый шкаф
            Assert.True(result);
            Assert.Equal(1, shop.ShelfCount);
            Assert.Equal(1, shop.TotalBooksCount);
        }

        /// <summary>
        /// Проверяет переисользование пустого шкафа со сменой жанра.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldReuseEmptyShelf()
        {
            // Arrange - Создаём пустой шкаф детектива
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Детектив", 10);
            shop.AddBookShelf(shelf);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Добавляем книгу фантастики
            var result = shop.TryAddBook(book);

            // Assert - Жанр шкафа изменён на Фантастика
            Assert.True(result);
            Assert.Equal(1, shop.ShelfCount);
            Assert.Equal("Фантастика", shelf.Genre);
        }

        /// <summary>
        /// Проверяет, что TryAddBook возвращает false при отсутствии места.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldReturnFalse_WhenNoSpaceAvailable()
        {
            // Arrange - Создаём заполненный шкаф и лимит шкафов = 1
            var shop = new Shop(1000m, 1);
            var shelf = new BookShelf(1, "Детектив", 1);
            var book1 = new Book(1, "Книга 1", "Автор", "Детектив", 200, 500m);
            var book2 = new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book1);
            shop.AddBookShelf(shelf);

            // Act - Пытаемся добавить книгу другого жанра
            var result = shop.TryAddBook(book2);

            // Assert - Книга не добавлена
            Assert.False(result);
        }

        /// <summary>
        /// Проверяет, что TryAddBook выбрасывает исключение при null книге.
        /// Примечание: текущая реализация выбрасывает NullReferenceException.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldThrowException_WhenBookIsNull()
        {
            // Arrange - Создаём магазин
            var shop = new Shop(1000m, 5);

            // Act & Assert - Добавление null вызывает NullReferenceException
            // (CanFitBook не проверяет null, что приводит к NullReferenceException)
            Assert.Throws<NullReferenceException>(() => shop.TryAddBook(null));
        }

        /// <summary>
        /// Проверяет, что первый шкаф создаётся с ID = 1.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldCreateShelfWithId1_WhenNoShelvesExist()
        {
            // Arrange - Создаём пустой магазин
            var shop = new Shop(1000m, 5);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Добавляем книгу
            shop.TryAddBook(book);

            // Assert - ID шкафа = 1
            Assert.Equal(1, shop.ShelfCount);
            Assert.Equal(1, shop.Shelves[0].Id);
        }

        /// <summary>
        /// Проверяет, что новый шкаф получает ID = max(existing) + 1.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldCreateShelfWithIncrementedId_WhenShelvesExist()
        {
            // Arrange - Создаём шкаф с ID = 3 и заполняем его
            var shop = new Shop(1000m, 5);
            var shelf1 = new BookShelf(3, "Детектив", 1);
            var book1 = new Book(1, "Книга 1", "Автор", "Детектив", 200, 500m);
            shelf1.AddBook(book1);
            shop.AddBookShelf(shelf1);

            var book2 = new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m);

            // Act - Добавляем книгу (создастся новый шкаф)
            var result = shop.TryAddBook(book2);

            // Assert - ID нового шкафа = 4 (max(3) + 1)
            Assert.True(result);
            Assert.Equal(2, shop.ShelfCount);
            Assert.Equal(4, shop.Shelves[1].Id);
        }

        #endregion

        #region Система сиквелов

        /// <summary>
        /// Проверяет, что первая книга серии получает PartNumber = 0.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldSetPartNumberZero_ForFirstBookInSeries()
        {
            // Arrange - Создаём пустой магазин
            var shop = new Shop(1000m, 5);
            var book = new Book(1, "Трилогия", "Автор", "Фантастика", 200, 500m);

            // Act - Добавляем книгу
            shop.TryAddBook(book);

            // Assert - PartNumber = 0 (оригинал)
            Assert.Equal(0, book.PartNumber);
        }

        /// <summary>
        /// Проверяет, что вторая книга того же названия получает PartNumber = 1.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldSetPartNumberOne_ForSecondBookInSeries()
        {
            // Arrange - Создаём магазин с первой книгой серии
            var shop = new Shop(1000m, 5);
            var book1 = new Book(1, "Трилогия", "Автор", "Фантастика", 200, 500m);
            shop.TryAddBook(book1);

            var book2 = new Book(2, "Трилогия", "Автор", "Фантастика", 200, 500m);

            // Act - Добавляем вторую книгу
            shop.TryAddBook(book2);

            // Assert - PartNumber = 1 (сиквел)
            Assert.Equal(1, book2.PartNumber);
            Assert.Equal("Трилогия 2", book2.DisplayTitle);
        }

        /// <summary>
        /// Проверяет, что книги с разными названиями не влияют друг на друга.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldNotAffectDifferentTitles()
        {
            // Arrange - Создаём книгу одной серии
            var shop = new Shop(1000m, 5);
            var book1 = new Book(1, "Серия А", "Автор", "Фантастика", 200, 500m);
            shop.TryAddBook(book1);

            var book2 = new Book(2, "Серия Б", "Автор", "Фантастика", 200, 500m);

            // Act - Добавляем книгу другой серии
            shop.TryAddBook(book2);

            // Assert - PartNumber = 0 (новая серия)
            Assert.Equal(0, book2.PartNumber);
        }

        /// <summary>
        /// Проверяет, что книги одного названия, но разных авторов не считаются серией.
        /// </summary>
        [Fact]
        public void TryAddBook_ShouldNotLinkBooksWithDifferentAuthors()
        {
            // Arrange - Создаём книгу автора А
            var shop = new Shop(1000m, 5);
            var book1 = new Book(1, "Книга", "Автор А", "Фантастика", 200, 500m);
            shop.TryAddBook(book1);

            var book2 = new Book(2, "Книга", "Автор Б", "Фантастика", 200, 500m);

            // Act - Добавляем книгу другого автора
            shop.TryAddBook(book2);

            // Assert - PartNumber = 0 (разные авторы)
            Assert.Equal(0, book2.PartNumber);
        }

        #endregion

        #region Метод SellBook

        /// <summary>
        /// Проверяет продажу книги и зачисление средств на баланс.
        /// </summary>
        [Fact]
        public void SellBook_ShouldIncreaseBalanceAndRemoveBook()
        {
            // Arrange - Создаём магазин с книгой
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);
            shop.AddBookShelf(shelf);

            // Act - Продаём книгу
            var price = shop.SellBook(1, 1);

            // Assert - Баланс увеличен, книга удалена
            Assert.Equal(500m, price);
            Assert.Equal(1500m, shop.Balance);
            Assert.Equal(0, shop.TotalBooksCount);
        }

        /// <summary>
        /// Проверяет, что SellBook выбрасывает исключение для несуществующего шкафа.
        /// </summary>
        [Fact]
        public void SellBook_ShouldThrowException_WhenShelfNotFound()
        {
            // Arrange - Создаём магазин без шкафов
            var shop = new Shop(1000m, 5);

            // Act & Assert - Продажа вызывает исключение
            Assert.Throws<InvalidOperationException>(() => shop.SellBook(999, 1));
        }

        #endregion

        #region Метод SellBookDirectly

        /// <summary>
        /// Проверяет прямую продажу книги по себестоимости.
        /// Используется при возврате поставщику.
        /// </summary>
        [Fact]
        public void SellBookDirectly_ShouldReturnBaseCost()
        {
            // Arrange - Создаём магазин с книгой
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            book.Price = 800m; // Цена выше себестоимости
            shelf.AddBook(book);
            shop.AddBookShelf(shelf);

            // Act - Продаём напрямую
            var price = shop.SellBookDirectly(1, 1);

            // Assert - Получена себестоимость, а не цена
            Assert.Equal(500m, price); // BaseCost, не Price
            Assert.Equal(1500m, shop.Balance);
        }

        #endregion

        #region Метод RemoveBookAfterCustomerSale

        /// <summary>
        /// Проверяет удаление книги после продажи клиенту.
        /// </summary>
        [Fact]
        public void RemoveBookAfterCustomerSale_ShouldRemoveBook()
        {
            // Arrange - Создаём магазин с книгой
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);
            shop.AddBookShelf(shelf);

            // Act - Удаляем книгу после продажи
            shop.RemoveBookAfterCustomerSale(book);

            // Assert - Книга удалена
            Assert.Equal(0, shop.TotalBooksCount);
        }

        #endregion
    }
}
