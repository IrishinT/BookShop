using BookShopLibrary;
using Xunit;

namespace BookShop.Tests
{
    /// <summary>
    /// Класс тестов для проверки функциональности класса BookShelf.
    /// Содержит тесты конструктора, методов добавления/удаления книг,
    /// поиска, продажи, изменения жанра и свойств состояния шкафа.
    /// Проверяет бизнес-логику хранения книг одного жанра.
    /// </summary>
    public class BookShelfTests
    {
        #region Конструктор и свойства

        /// <summary>
        /// Проверяет, что конструктор создаёт шкаф с корректными свойствами.
        /// Шкаф должен быть пустым, с указанными ID, жанром и вместимостью.
        /// </summary>
        [Fact]
        public void Constructor_ShouldCreateBookShelfWithCorrectProperties()
        {
            // Arrange & Act - Создаём шкаф с тестовыми параметрами
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Assert - Проверяем все свойства
            Assert.Equal(1, shelf.Id);                    // ID должен быть 1
            Assert.Equal("Фантастика", shelf.Genre);      // Жанр сохранён
            Assert.Equal(10, shelf.Capacity);             // Вместимость = 10
            Assert.Equal(0, shelf.CurrentCount);          // Шкаф пуст
            Assert.True(shelf.IsEmpty);                   // Флаг пустоты = true
            Assert.True(shelf.HasFreeSpace);              // Есть свободное место
        }

        /// <summary>
        /// Проверяет, что шкаф корректно создаётся с минимальной вместимостью (1 книга).
        /// Это граничный случай для проверки логики заполнения.
        /// </summary>
        [Fact]
        public void Constructor_ShouldAcceptMinimumCapacity()
        {
            // Arrange & Act - Создаём шкаф на 1 книгу
            var shelf = new BookShelf(1, "Жанр", 1);

            // Assert - Вместимость = 1, шкаф пуст
            Assert.Equal(1, shelf.Capacity);
            Assert.True(shelf.HasFreeSpace);
        }

        /// <summary>
        /// Проверяет, что шкаф корректно создаётся с большой вместимостью.
        /// Убеждается, что нет ограничений на максимальный размер.
        /// </summary>
        [Fact]
        public void Constructor_ShouldAcceptLargeCapacity()
        {
            // Arrange & Act - Создаём шкаф на много книг
            var shelf = new BookShelf(1, "Жанр", 1000);

            // Assert - Вместимость сохранена корректно
            Assert.Equal(1000, shelf.Capacity);
        }

        #endregion

        #region Метод AddBook

        /// <summary>
        /// Проверяет успешное добавление книги в пустой шкаф.
        /// Книга должна добавиться, счётчик должен увеличиться.
        /// </summary>
        [Fact]
        public void AddBook_ShouldAddBookSuccessfully()
        {
            // Arrange - Создаём пустой шкаф и книгу того же жанра
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Добавляем книгу
            var result = shelf.AddBook(book);

            // Assert - Книга добавлена успешно
            Assert.True(result);                    // Метод вернул true
            Assert.Equal(1, shelf.CurrentCount);    // Счётчик = 1
            Assert.False(shelf.IsEmpty);            // Шкаф не пуст
        }

        /// <summary>
        /// Проверяет, что при добавлении книги несоответствующего жанра
        /// выбрасывается исключение InvalidOperationException.
        /// </summary>
        [Fact]
        public void AddBook_ShouldThrowException_WhenGenreMismatch()
        {
            // Arrange - Создаём шкаф для фантастики и книгу детектива
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Детектив", 200, 500m);

            // Act & Assert - Должно выброситься исключение
            var exception = Assert.Throws<InvalidOperationException>(() => shelf.AddBook(book));
            Assert.Contains("не соответствует жанру шкафа", exception.Message);
        }

        /// <summary>
        /// Проверяет, что при попытке добавить книгу в полный шкаф
        /// метод возвращает false и не добавляет книгу.
        /// </summary>
        [Fact]
        public void AddBook_ShouldReturnFalse_WhenShelfIsFull()
        {
            // Arrange - Создаём шкаф на 2 книги и заполняем его
            var shelf = new BookShelf(1, "Фантастика", 2);
            var book1 = new Book(1, "Книга 1", "Автор", "Фантастика", 200, 500m);
            var book2 = new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m);
            var book3 = new Book(3, "Книга 3", "Автор", "Фантастика", 200, 500m);

            shelf.AddBook(book1);
            shelf.AddBook(book2);

            // Act - Пытаемся добавить третью книгу
            var result = shelf.AddBook(book3);

            // Assert - Книга не добавлена
            Assert.False(result);                   // Метод вернул false
            Assert.Equal(2, shelf.CurrentCount);    // Осталось 2 книги
            Assert.False(shelf.HasFreeSpace);       // Нет свободного места
        }

        /// <summary>
        /// Проверяет, что при попытке добавить null выбрасывается ArgumentNullException.
        /// Это защита от некорректных данных.
        /// </summary>
        [Fact]
        public void AddBook_ShouldThrowException_WhenBookIsNull()
        {
            // Arrange - Создаём пустой шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act & Assert - Добавление null должно вызвать исключение
            Assert.Throws<ArgumentNullException>(() => shelf.AddBook(null));
        }

        /// <summary>
        /// Проверяет добавление нескольких книг одного жанра.
        /// Все книги должны добавляться успешно до заполнения шкафа.
        /// </summary>
        [Fact]
        public void AddBook_ShouldAddMultipleBooksOfSameGenre()
        {
            // Arrange - Создаём шкаф и несколько книг одного жанра
            var shelf = new BookShelf(1, "Фантастика", 5);

            // Act - Добавляем 5 книг
            for (int i = 1; i <= 5; i++)
            {
                var book = new Book(i, $"Книга {i}", "Автор", "Фантастика", 200, 500m);
                var result = shelf.AddBook(book);
                Assert.True(result);  // Каждая книга должна добавиться успешно
            }

            // Assert - Шкаф заполнен
            Assert.Equal(5, shelf.CurrentCount);
            Assert.False(shelf.HasFreeSpace);
        }

        #endregion

        #region Метод RemoveBook

        /// <summary>
        /// Проверяет успешное удаление книги из шкафа по ID.
        /// После удаления счётчик должен уменьшиться.
        /// </summary>
        [Fact]
        public void RemoveBook_ShouldRemoveBookSuccessfully()
        {
            // Arrange - Создаём шкаф с одной книгой
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);

            // Act - Удаляем книгу по ID
            var result = shelf.RemoveBook(1);

            // Assert - Книга удалена
            Assert.True(result);                 // Метод вернул true
            Assert.Equal(0, shelf.CurrentCount); // Шкаф пуст
            Assert.True(shelf.IsEmpty);          // Флаг пустоты = true
        }

        /// <summary>
        /// Проверяет, что при попытке удалить несуществующую книгу
        /// метод возвращает false и не изменяет состояние шкафа.
        /// </summary>
        [Fact]
        public void RemoveBook_ShouldReturnFalse_WhenBookNotFound()
        {
            // Arrange - Создаём пустой шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act - Пытаемся удалить несуществующую книгу
            var result = shelf.RemoveBook(999);

            // Assert - Удаление не произошло
            Assert.False(result);
        }

        /// <summary>
        /// Проверяет удаление книги из середины списка.
        /// Остальные книги должны остаться на месте.
        /// </summary>
        [Fact]
        public void RemoveBook_ShouldRemoveCorrectBook_WhenMultipleBooksExist()
        {
            // Arrange - Создаём шкаф с тремя книгами
            var shelf = new BookShelf(1, "Фантастика", 10);
            shelf.AddBook(new Book(1, "Книга 1", "Автор", "Фантастика", 200, 500m));
            shelf.AddBook(new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m));
            shelf.AddBook(new Book(3, "Книга 3", "Автор", "Фантастика", 200, 500m));

            // Act - Удаляем книгу с ID = 2
            var result = shelf.RemoveBook(2);

            // Assert - Книга удалена, осталось 2 книги
            Assert.True(result);
            Assert.Equal(2, shelf.CurrentCount);
            Assert.Null(shelf.FindBookById(2));  // Книга с ID=2 не найдена
            Assert.NotNull(shelf.FindBookById(1)); // Книга с ID=1 на месте
            Assert.NotNull(shelf.FindBookById(3)); // Книга с ID=3 на месте
        }

        #endregion

        #region Метод FindBookByTitle

        /// <summary>
        /// Проверяет поиск книги по названию, когда книга существует.
        /// Должна вернуться книга с указанным названием.
        /// </summary>
        [Fact]
        public void FindBookByTitle_ShouldReturnBook_WhenExists()
        {
            // Arrange - Создаём шкаф с книгой
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Война миров", "Уэллс", "Фантастика", 300, 700m);
            shelf.AddBook(book);

            // Act - Ищем книгу по названию
            var found = shelf.FindBookByTitle("Война миров");

            // Assert - Книга найдена
            Assert.NotNull(found);
            Assert.Equal("Война миров", found.Title);
        }

        /// <summary>
        /// Проверяет, что поиск по названию работает без учёта регистра.
        /// "ВОЙНА МИРОВ" и "война миров" должны находить одну книгу.
        /// </summary>
        [Theory]
        [InlineData("ВОЙНА МИРОВ")]   // Верхний регистр
        [InlineData("война миров")]    // Нижний регистр
        [InlineData("Война Миров")]    // Смешанный регистр
        public void FindBookByTitle_ShouldBeCaseInsensitive(string searchTitle)
        {
            // Arrange - Создаём шкаф с книгой
            var shelf = new BookShelf(1, "Фантастика", 10);
            shelf.AddBook(new Book(1, "Война миров", "Уэллс", "Фантастика", 300, 700m));

            // Act - Ищем книгу с разным регистром
            var found = shelf.FindBookByTitle(searchTitle);

            // Assert - Книга найдена независимо от регистра
            Assert.NotNull(found);
        }

        /// <summary>
        /// Проверяет, что при поиске несуществующей книги возвращается null.
        /// </summary>
        [Fact]
        public void FindBookByTitle_ShouldReturnNull_WhenNotFound()
        {
            // Arrange - Создаём шкаф с книгой
            var shelf = new BookShelf(1, "Фантастика", 10);
            shelf.AddBook(new Book(1, "Книга", "Автор", "Фантастика", 200, 500m));

            // Act - Ищем несуществующую книгу
            var found = shelf.FindBookByTitle("Несуществующая книга");

            // Assert - Результат null
            Assert.Null(found);
        }

        /// <summary>
        /// Проверяет, что при передаче пустой строки возвращается null.
        /// Это защита от некорректного ввода.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void FindBookByTitle_ShouldReturnNull_WhenTitleIsEmpty(string? title)
        {
            // Arrange - Создаём шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act - Ищем по пустой строке
            var result = shelf.FindBookByTitle(title);

            // Assert - Результат null
            Assert.Null(result);
        }

        #endregion

        #region Метод FindBookById

        /// <summary>
        /// Проверяет поиск книги по ID, когда книга существует.
        /// Должна вернуться книга с указанным ID.
        /// </summary>
        [Fact]
        public void FindBookById_ShouldReturnBook_WhenExists()
        {
            // Arrange - Создаём шкаф с книгой
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(5, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);

            // Act - Ищем книгу по ID = 5
            var found = shelf.FindBookById(5);

            // Assert - Книга найдена
            Assert.NotNull(found);
            Assert.Equal(5, found.Id);
        }

        /// <summary>
        /// Проверяет, что при поиске несуществующего ID возвращается null.
        /// </summary>
        [Fact]
        public void FindBookById_ShouldReturnNull_WhenNotFound()
        {
            // Arrange - Создаём пустой шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act - Ищем несуществующий ID
            var found = shelf.FindBookById(999);

            // Assert - Результат null
            Assert.Null(found);
        }

        /// <summary>
        /// Проверяет поиск книги с ID = 0 (граничный случай).
        /// </summary>
        [Fact]
        public void FindBookById_ShouldFindBookWithZeroId()
        {
            // Arrange - Создаём книгу с ID = 0
            var shelf = new BookShelf(1, "Фантастика", 10);
            shelf.AddBook(new Book(0, "Книга", "Автор", "Фантастика", 200, 500m));

            // Act - Ищем ID = 0
            var found = shelf.FindBookById(0);

            // Assert - Книга найдена
            Assert.NotNull(found);
            Assert.Equal(0, found.Id);
        }

        #endregion

        #region Метод GetAllBooks

        /// <summary>
        /// Проверяет, что GetAllBooks возвращает список всех книг в шкафу.
        /// Список должен быть отсортирован по ID.
        /// </summary>
        [Fact]
        public void GetAllBooks_ShouldReturnListOfBooks()
        {
            // Arrange - Создаём шкаф с несколькими книгами
            var shelf = new BookShelf(1, "Фантастика", 10);
            shelf.AddBook(new Book(3, "Книга 3", "Автор", "Фантастика", 200, 500m));
            shelf.AddBook(new Book(1, "Книга 1", "Автор", "Фантастика", 200, 500m));
            shelf.AddBook(new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m));

            // Act - Получаем все книги
            var books = shelf.GetAllBooks();

            // Assert - Список содержит 3 книги, отсортированные по ID
            Assert.Equal(3, books.Count);
            Assert.Equal(1, books[0].Id);  // Сортировка по ID
            Assert.Equal(2, books[1].Id);
            Assert.Equal(3, books[2].Id);
        }

        /// <summary>
        /// Проверяет, что GetAllBooks возвращает пустой список для пустого шкафа.
        /// </summary>
        [Fact]
        public void GetAllBooks_ShouldReturnEmptyList_WhenShelfIsEmpty()
        {
            // Arrange - Создаём пустой шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act - Получаем все книги
            var books = shelf.GetAllBooks();

            // Assert - Список пуст
            Assert.Empty(books);
        }

        /// <summary>
        /// Проверяет, что GetAllBooks возвращает копию списка,
        /// а не ссылку на внутренний список.
        /// Изменения в возвращённом списке не должны влиять на шкаф.
        /// </summary>
        [Fact]
        public void GetAllBooks_ShouldReturnNewList()
        {
            // Arrange - Создаём шкаф с книгой
            var shelf = new BookShelf(1, "Фантастика", 10);
            shelf.AddBook(new Book(1, "Книга", "Автор", "Фантастика", 200, 500m));

            // Act - Получаем список и очищаем его
            var books = shelf.GetAllBooks();
            books.Clear();

            // Assert - В шкафу книга осталась
            Assert.Equal(1, shelf.CurrentCount);
        }

        #endregion

        #region Метод SellBook

        /// <summary>
        /// Проверяет, что продажа книги удаляет её из шкафа и возвращает цену.
        /// </summary>
        [Fact]
        public void SellBook_ShouldRemoveBookAndReturnPrice()
        {
            // Arrange - Создаём шкаф с книгой
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 1000m);
            shelf.AddBook(book);

            // Act - Продаём книгу
            var price = shelf.SellBook(1);

            // Assert - Книга удалена, цена возвращена
            Assert.Equal(1000m, price);
            Assert.Equal(0, shelf.CurrentCount);
        }

        /// <summary>
        /// Проверяет, что продажа возвращает текущую цену книги (Price),
        /// а не себестоимость (BaseCost).
        /// </summary>
        [Fact]
        public void SellBook_ShouldReturnCurrentPrice()
        {
            // Arrange - Создаём книгу и изменяем цену
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            book.Price = 800m; // Устанавливаем цену выше себестоимости
            shelf.AddBook(book);

            // Act - Продаём книгу
            var price = shelf.SellBook(1);

            // Assert - Возвращена текущая цена
            Assert.Equal(800m, price);
        }

        /// <summary>
        /// Проверяет, что при попытке продать несуществующую книгу
        /// выбрасывается исключение InvalidOperationException.
        /// </summary>
        [Fact]
        public void SellBook_ShouldThrowException_WhenBookNotFound()
        {
            // Arrange - Создаём пустой шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act & Assert - Продажа несуществующей книги вызывает исключение
            Assert.Throws<InvalidOperationException>(() => shelf.SellBook(999));
        }

        #endregion

        #region Метод ChangeGenre

        /// <summary>
        /// Проверяет успешное изменение жанра пустого шкафа.
        /// Это единственный случай, когда смена жанра разрешена.
        /// </summary>
        [Fact]
        public void ChangeGenre_ShouldChangeGenre_WhenShelfIsEmpty()
        {
            // Arrange - Создаём пустой шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act - Меняем жанр
            shelf.ChangeGenre("Детектив");

            // Assert - Жанр изменён
            Assert.Equal("Детектив", shelf.Genre);
        }

        /// <summary>
        /// Проверяет, что изменение жанра непустого шкафа
        /// выбрасывает исключение InvalidOperationException.
        /// </summary>
        [Fact]
        public void ChangeGenre_ShouldThrowException_WhenShelfIsNotEmpty()
        {
            // Arrange - Создаём шкаф с книгой
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);

            // Act & Assert - Попытка смены жанра вызывает исключение
            Assert.Throws<InvalidOperationException>(() => shelf.ChangeGenre("Детектив"));
        }

        /// <summary>
        /// Проверяет, что при передаче пустой строки или null
        /// выбрасывается исключение ArgumentException.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ChangeGenre_ShouldThrowException_WhenGenreIsEmpty(string newGenre)
        {
            // Arrange - Создаём пустой шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act & Assert - Пустой жанр вызывает исключение
            Assert.Throws<ArgumentException>(() => shelf.ChangeGenre(newGenre));
        }

        /// <summary>
        /// Проверяет возможность многократной смены жанра пустого шкафа.
        /// </summary>
        [Fact]
        public void ChangeGenre_ShouldAllowMultipleChanges_WhenShelfIsEmpty()
        {
            // Arrange - Создаём пустой шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act - Меняем жанр несколько раз
            shelf.ChangeGenre("Детектив");
            shelf.ChangeGenre("Роман");
            shelf.ChangeGenre("Фэнтези");

            // Assert - Последний жанр сохранён
            Assert.Equal("Фэнтези", shelf.Genre);
        }

        #endregion

        #region Свойства IsEmpty и HasFreeSpace

        /// <summary>
        /// Проверяет, что IsEmpty возвращает true для нового шкафа.
        /// </summary>
        [Fact]
        public void IsEmpty_ShouldReturnTrue_WhenShelfIsNew()
        {
            // Arrange & Act - Создаём новый шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Assert - Шкаф пуст
            Assert.True(shelf.IsEmpty);
        }

        /// <summary>
        /// Проверяет, что IsEmpty возвращает false после добавления книги.
        /// </summary>
        [Fact]
        public void IsEmpty_ShouldReturnFalse_AfterAddingBook()
        {
            // Arrange - Создаём шкаф и добавляем книгу
            var shelf = new BookShelf(1, "Фантастика", 10);
            shelf.AddBook(new Book(1, "Книга", "Автор", "Фантастика", 200, 500m));

            // Assert - Шкаф не пуст
            Assert.False(shelf.IsEmpty);
        }

        /// <summary>
        /// Проверяет, что HasFreeSpace возвращает true для нового шкафа.
        /// </summary>
        [Fact]
        public void HasFreeSpace_ShouldReturnTrue_WhenShelfIsNew()
        {
            // Arrange & Act - Создаём новый шкаф
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Assert - Есть место
            Assert.True(shelf.HasFreeSpace);
        }

        /// <summary>
        /// Проверяет, что HasFreeSpace возвращает false для полного шкафа.
        /// </summary>
        [Fact]
        public void HasFreeSpace_ShouldReturnFalse_WhenShelfIsFull()
        {
            // Arrange - Создаём шкаф на 1 книгу и заполняем его
            var shelf = new BookShelf(1, "Фантастика", 1);
            shelf.AddBook(new Book(1, "Книга", "Автор", "Фантастика", 200, 500m));

            // Assert - Нет места
            Assert.False(shelf.HasFreeSpace);
        }

        /// <summary>
        /// Проверяет, что HasFreeSpace снова true после удаления книги.
        /// </summary>
        [Fact]
        public void HasFreeSpace_ShouldReturnTrue_AfterRemovingBook()
        {
            // Arrange - Создаём шкаф на 1 книгу, заполняем и удаляем
            var shelf = new BookShelf(1, "Фантастика", 1);
            shelf.AddBook(new Book(1, "Книга", "Автор", "Фантастика", 200, 500m));
            shelf.RemoveBook(1);

            // Assert - Снова есть место
            Assert.True(shelf.HasFreeSpace);
        }

        #endregion

        #region Метод ToString

        /// <summary>
        /// Проверяет, что ToString возвращает строку в ожидаемом формате.
        /// Формат: "Шкаф ID (Жанр) - ТекущееКоличество/Вместимость"
        /// </summary>
        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange - Создаём шкаф с книгой
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);

            // Act - Получаем строковое представление
            var result = shelf.ToString();

            // Assert - Формат: "Шкаф 1 (Фантастика) - 1/10"
            Assert.Equal("Шкаф 1 (Фантастика) - 1/10", result);
        }

        /// <summary>
        /// Проверяет ToString для пустого шкафа.
        /// </summary>
        [Fact]
        public void ToString_ShouldShowZeroBooks_WhenShelfIsEmpty()
        {
            // Arrange - Создаём пустой шкаф
            var shelf = new BookShelf(5, "Детектив", 20);

            // Act
            var result = shelf.ToString();

            // Assert - Формат: "Шкаф 5 (Детектив) - 0/20"
            Assert.Equal("Шкаф 5 (Детектив) - 0/20", result);
        }

        /// <summary>
        /// Проверяет ToString для полного шкафа.
        /// </summary>
        [Fact]
        public void ToString_ShouldShowFullCapacity_WhenShelfIsFull()
        {
            // Arrange - Создаём и заполняем шкаф
            var shelf = new BookShelf(1, "Фантастика", 2);
            shelf.AddBook(new Book(1, "Книга 1", "Автор", "Фантастика", 200, 500m));
            shelf.AddBook(new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m));

            // Act
            var result = shelf.ToString();

            // Assert - Формат: "Шкаф 1 (Фантастика) - 2/2"
            Assert.Equal("Шкаф 1 (Фантастика) - 2/2", result);
        }

        #endregion

        #region Комплексные тесты

        /// <summary>
        /// Комплексный тест: заполнение шкафа книгами, проверка состояний,
        /// удаление нескольких книг и повторное добавление.
        /// </summary>
        [Fact]
        public void BookShelf_ShouldHandleMultipleOperationsCorrectly()
        {
            // Arrange - Создаём шкаф на 5 книг
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act 1 - Добавляем 3 книги
            for (int i = 1; i <= 3; i++)
            {
                shelf.AddBook(new Book(i, $"Книга {i}", "Автор", "Фантастика", 200, 500m));
            }

            // Assert 1 - 3 книги в шкафу
            Assert.Equal(3, shelf.CurrentCount);
            Assert.True(shelf.HasFreeSpace);

            // Act 2 - Удаляем книгу из середины
            shelf.RemoveBook(2);

            // Assert 2 - 2 книги, книга с ID=2 не найдена
            Assert.Equal(2, shelf.CurrentCount);
            Assert.Null(shelf.FindBookById(2));

            // Act 3 - Добавляем новую книгу
            shelf.AddBook(new Book(10, "Новая книга", "Автор", "Фантастика", 200, 500m));

            // Assert 3 - 3 книги, новая книга найдена
            Assert.Equal(3, shelf.CurrentCount);
            Assert.NotNull(shelf.FindBookById(10));

            // Act 4 - Продаём книгу
            var price = shelf.SellBook(1);

            // Assert 4 - 2 книги, получена цена
            Assert.Equal(500m, price);
            Assert.Equal(2, shelf.CurrentCount);
        }

        #endregion
    }
}
