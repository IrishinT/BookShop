using BookShopLibrary;
using Xunit;

namespace BookShop.Tests
{
    /// <summary>
    /// Класс тестов для проверки функциональности класса Book.
    /// Содержит тесты конструктора, свойств, методов Clone, Sell, GenerateRandom и ToString.
    /// Проверяет корректность создания книг, систему сиквелов и валидацию данных.
    /// </summary>
    public class BookTests
    {
        #region Конструктор и свойства

        /// <summary>
        /// Проверяет, что конструктор книги создаёт объект с корректными значениями всех свойств.
        /// Убеждается, что все переданные параметры сохраняются без изменений,
        /// а Price по умолчанию равна BaseCost.
        /// </summary>
        [Fact]
        public void Constructor_ShouldCreateBookWithCorrectProperties()
        {
            // Arrange & Act - Подготовка данных и выполнение действия
            // Создаём книгу с тестовыми данными
            var book = new Book(1, "Тестовая книга", "Тестовый автор", "Фантастика", 300, 500m);

            // Assert - Проверка результатов
            // Все свойства должны соответствовать переданным значениям
            Assert.Equal(1, book.Id);                           // ID должен быть 1
            Assert.Equal("Тестовая книга", book.Title);         // Название сохранено
            Assert.Equal("Тестовый автор", book.Author);        // Автор сохранён
            Assert.Equal("Фантастика", book.Genre);             // Жанр сохранён
            Assert.Equal(300, book.Pages);                      // Количество страниц
            Assert.Equal(500m, book.BaseCost);                  // Себестоимость
            Assert.Equal(500m, book.Price);                     // Цена продажи = себестоимость
            Assert.Equal(0, book.PartNumber);                   // Номер части = 0 (оригинал)
        }

        /// <summary>
        /// Проверяет, что конструктор с параметром partNumber корректно устанавливает номер части.
        /// Это важно для системы сиквелов (продолжений книг).
        /// </summary>
        [Fact]
        public void Constructor_WithPartNumber_ShouldSetCorrectPartNumber()
        {
            // Arrange & Act - Создаём книгу с указанием номера части
            var book = new Book(1, "Трилогия", "Автор", "Фантастика", 300, 500m, 2);

            // Assert - Номер части должен быть 2 (третья книга в серии)
            Assert.Equal(2, book.PartNumber);
        }

        /// <summary>
        /// Проверяет, что свойство DisplayTitle корректно отображает название для оригинала.
        /// Для книг с PartNumber = 0 должно возвращаться просто Title.
        /// </summary>
        [Fact]
        public void DisplayTitle_ShouldReturnTitle_WhenPartNumberIsZero()
        {
            // Arrange - Создаём оригинальную книгу (не сиквел)
            var book = new Book(1, "Война миров", "Уэллс", "Фантастика", 300, 500m, 0);

            // Act - Получаем отображаемое название
            var displayTitle = book.DisplayTitle;

            // Assert - Должно быть просто название без номера
            Assert.Equal("Война миров", displayTitle);
        }

        /// <summary>
        /// Проверяет, что свойство DisplayTitle корректно отображает название для сиквела.
        /// Для книг с PartNumber > 0 должно возвращаться "Название N", где N = PartNumber + 1.
        /// </summary>
        [Theory]
        [InlineData(1, "Трилогия 2")]   // PartNumber = 1 -> отображается как "2"
        [InlineData(2, "Трилогия 3")]   // PartNumber = 2 -> отображается как "3"
        [InlineData(5, "Трилогия 6")]   // PartNumber = 5 -> отображается как "6"
        public void DisplayTitle_ShouldReturnTitleWithNumber_WhenPartNumberIsGreaterThanZero(int partNumber, string expected)
        {
            // Arrange - Создаём книгу с указанным номером части
            var book = new Book(1, "Трилогия", "Автор", "Фантастика", 300, 500m, partNumber);

            // Act - Получаем отображаемое название
            var displayTitle = book.DisplayTitle;

            // Assert - Название должно содержать номер части + 1
            Assert.Equal(expected, displayTitle);
        }

        #endregion

        #region Метод Clone

        /// <summary>
        /// Проверяет, что метод Clone создаёт точную копию книги.
        /// Все свойства копии должны совпадать с оригиналом,
        /// но это должен быть новый объект.
        /// </summary>
        [Fact]
        public void Clone_ShouldCreateExactCopy()
        {
            // Arrange - Создаём оригинальную книгу с сиквелом
            var original = new Book(10, "Клон-тест", "Автор", "Детектив", 400, 750m, 3);

            // Act - Создаём клон
            var clone = original.Clone();

            // Assert - Все свойства должны совпадать
            Assert.Equal(original.Id, clone.Id);
            Assert.Equal(original.Title, clone.Title);
            Assert.Equal(original.Author, clone.Author);
            Assert.Equal(original.Genre, clone.Genre);
            Assert.Equal(original.Pages, clone.Pages);
            Assert.Equal(original.BaseCost, clone.BaseCost);
            Assert.Equal(original.PartNumber, clone.PartNumber);

            // Но это должны быть разные объекты
            Assert.NotSame(original, clone);
        }

        /// <summary>
        /// Проверяет, что изменения в клоне не влияют на оригинал.
        /// Это подтверждает, что клонирование создаёт независимую копию.
        /// </summary>
        [Fact]
        public void Clone_ShouldCreateIndependentCopy()
        {
            // Arrange - Создаём оригинальную книгу
            var original = new Book(1, "Оригинал", "Автор", "Жанр", 200, 500m);

            // Act - Создаём клон и изменяем его
            var clone = original.Clone();
            clone.Title = "Изменённый клон";
            clone.Price = 1000m;

            // Assert - Оригинал не должен измениться
            Assert.Equal("Оригинал", original.Title);
            Assert.Equal(500m, original.Price);
            Assert.Equal("Изменённый клон", clone.Title);
            Assert.Equal(1000m, clone.Price);
        }

        #endregion

        #region Метод Sell

        /// <summary>
        /// Проверяет, что метод Sell возвращает текущую цену продажи книги.
        /// Это значение должно зачисляться на баланс магазина при продаже.
        /// </summary>
        [Fact]
        public void Sell_ShouldReturnBookPrice()
        {
            // Arrange - Создаём книгу с известной ценой
            var book = new Book(1, "Книга", "Автор", "Жанр", 200, 1000m);

            // Act - Вызываем метод продажи
            var price = book.Sell();

            // Assert - Должна вернуться цена продажи
            Assert.Equal(1000m, price);
        }

        /// <summary>
        /// Проверяет, что метод Sell возвращает изменённую цену,
        /// если свойство Price было изменено после создания книги.
        /// </summary>
        [Fact]
        public void Sell_ShouldReturnModifiedPrice()
        {
            // Arrange - Создаём книгу и изменяем цену продажи
            var book = new Book(1, "Книга", "Автор", "Жанр", 200, 500m);
            book.Price = 750m; // Устанавливаем цену выше себестоимости

            // Act - Вызываем метод продажи
            var price = book.Sell();

            // Assert - Должна вернуться установленная цена
            Assert.Equal(750m, price);
        }

        #endregion

        #region Метод GenerateRandom (из файлов)

        /// <summary>
        /// Проверяет, что генерация случайной книги из файлов выбрасывает исключение,
        /// если файлы данных (titles.txt, authors.txt, genres.txt) не найдены.
        /// Это ожидаемое поведение при отсутствии файлов.
        /// </summary>
        [Fact]
        public void GenerateRandom_ShouldThrowException_WhenFilesNotFound()
        {
            // Act & Assert - При отсутствии файлов должно выброситься исключение
            var exception = Assert.Throws<Exception>(() => Book.GenerateRandom(1));
            Assert.Contains("Ошибка при генерации книги", exception.Message);
        }

        /// <summary>
        /// Проверяет успешную генерацию случайной книги из существующих файлов.
        /// Создаёт временные файлы с тестовыми данными и проверяет,
        /// что книга генерируется с правильными значениями.
        /// </summary>
        [Fact]
        public void GenerateRandom_ShouldGenerateBook_WhenFilesExist()
        {
            // Arrange - Создаём временные файлы с тестовыми данными
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(basePath);
            File.WriteAllLines(Path.Combine(basePath, "titles.txt"), new[] { "Тестовая книга" });
            File.WriteAllLines(Path.Combine(basePath, "authors.txt"), new[] { "Тестовый автор" });
            File.WriteAllLines(Path.Combine(basePath, "genres.txt"), new[] { "Тестовый жанр" });

            try
            {
                // Act - Генерируем случайную книгу
                var book = Book.GenerateRandom(100);

                // Assert - Проверяем, что книга создана с правильными данными
                Assert.NotNull(book);
                Assert.Equal(100, book.Id);                  // ID из параметра
                Assert.Equal("Тестовая книга", book.Title);  // Из файла
                Assert.Equal("Тестовый автор", book.Author); // Из файла
                Assert.Equal("Тестовый жанр", book.Genre);   // Из файла
                Assert.InRange(book.Pages, 100, 1000);       // Случайное в диапазоне
                Assert.InRange(book.BaseCost, 100m, 1000m);  // Случайное в диапазоне
            }
            finally
            {
                // Cleanup - Удаляем временные файлы
                File.Delete(Path.Combine(basePath, "titles.txt"));
                File.Delete(Path.Combine(basePath, "authors.txt"));
                File.Delete(Path.Combine(basePath, "genres.txt"));
            }
        }

        #endregion

        #region Метод GenerateRandom (из переданных данных)

        /// <summary>
        /// Проверяет, что генерация книги из переданных данных выбрасывает исключение,
        /// если список пар (книга, автор) пустой или null.
        /// </summary>
        [Fact]
        public void GenerateRandom_WithData_ShouldThrowException_WhenBookAuthorPairsIsEmpty()
        {
            // Arrange - Подготавливаем пустой список пар
            var genres = new List<string> { "Фантастика" };

            // Act & Assert - Должно выброситься исключение
            Assert.Throws<ArgumentException>(() => Book.GenerateRandom(1, new List<(string, string)>(), genres));
        }

        /// <summary>
        /// Проверяет, что генерация книги из переданных данных выбрасывает исключение,
        /// если список жанров пустой или null.
        /// </summary>
        [Fact]
        public void GenerateRandom_WithData_ShouldThrowException_WhenGenresIsEmpty()
        {
            // Arrange - Подготавливаем пустой список жанров
            var pairs = new List<(string Title, string Author)> { ("Книга", "Автор") };

            // Act & Assert - Должно выброситься исключение
            Assert.Throws<ArgumentException>(() => Book.GenerateRandom(1, pairs, new List<string>()));
        }

        /// <summary>
        /// Проверяет успешную генерацию книги из переданных данных.
        /// Книга должна создаваться из одного из элементов списков.
        /// </summary>
        [Fact]
        public void GenerateRandom_WithData_ShouldGenerateValidBook()
        {
            // Arrange - Подготавливаем тестовые данные
            var pairs = new List<(string Title, string Author)>
            {
                ("Книга 1", "Автор 1"),
                ("Книга 2", "Автор 2")
            };
            var genres = new List<string> { "Фантастика", "Детектив" };

            // Act - Генерируем книгу
            var book = Book.GenerateRandom(50, pairs, genres);

            // Assert - Проверяем корректность созданной книги
            Assert.Equal(50, book.Id);
            Assert.Contains(book.Title, pairs.Select(p => p.Title));       // Название из списка
            Assert.Contains(book.Author, pairs.Select(p => p.Author));     // Автор из списка
            Assert.Contains(book.Genre, genres);                           // Жанр из списка
            Assert.InRange(book.Pages, 100, 1000);                         // Случайные страницы
            Assert.InRange(book.BaseCost, 100m, 1000m);                    // Случайная себестоимость
            Assert.Equal(book.BaseCost, book.Price);                       // Цена = себестоимость
        }

        #endregion

        #region Метод ToString

        /// <summary>
        /// Проверяет, что метод ToString возвращает строку в ожидаемом формате.
        /// Формат: "ID: DisplayTitle - Author (Genre) | Себестоимость: BaseCost"
        /// </summary>
        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange - Создаём книгу с тестовыми данными
            var book = new Book(5, "Война и мир", "Толстой", "Классика", 1200, 1500m);

            // Act - Получаем строковое представление
            var result = book.ToString();

            // Assert - Строка должна содержать все ключевые элементы
            Assert.Contains("5", result);                           // ID
            Assert.Contains("Война и мир", result);                  // Название
            Assert.Contains("Толстой", result);                      // Автор
            Assert.Contains("Классика", result);                     // Жанр
        }

        /// <summary>
        /// Проверяет, что ToString корректно отображает сиквел книги.
        /// Для книги с PartNumber > 0 должно отображаться DisplayTitle.
        /// </summary>
        [Fact]
        public void ToString_ShouldShowDisplayTitleForSequel()
        {
            // Arrange - Создаём сиквел (вторая часть трилогии)
            var book = new Book(1, "Трилогия", "Автор", "Фантастика", 300, 500m, 1);

            // Act - Получаем строковое представление
            var result = book.ToString();

            // Assert - Должно отображаться "Трилогия 2" (номер части + 1)
            Assert.Contains("Трилогия 2", result);
        }

        #endregion

        #region Граничные случаи

        /// <summary>
        /// Проверяет, что книга может быть создана с минимальными допустимыми значениями.
        /// ID = 0, пустые строки, 0 страниц и 0 стоимость.
        /// </summary>
        [Fact]
        public void Constructor_ShouldAcceptMinimumValues()
        {
            // Arrange & Act - Создаём книгу с минимальными значениями
            var book = new Book(0, "", "", "", 0, 0m);

            // Assert - Книга создаётся без ошибок
            Assert.Equal(0, book.Id);
            Assert.Equal("", book.Title);
            Assert.Equal("", book.Author);
            Assert.Equal("", book.Genre);
            Assert.Equal(0, book.Pages);
            Assert.Equal(0m, book.BaseCost);
        }

        /// <summary>
        /// Проверяет, что книга может быть создана с большими значениями.
        /// Это проверяет, что нет неожиданных ограничений.
        /// </summary>
        [Fact]
        public void Constructor_ShouldAcceptLargeValues()
        {
            // Arrange & Act - Создаём книгу с большими значениями
            var book = new Book(
                id: int.MaxValue,
                title: new string('А', 1000),  // Длинное название
                author: new string('Б', 1000), // Длинный автор
                genre: "Фантастика",
                pages: int.MaxValue,
                baseCost: decimal.MaxValue
            );

            // Assert - Все значения сохранены корректно
            Assert.Equal(int.MaxValue, book.Id);
            Assert.Equal(1000, book.Title.Length);
            Assert.Equal(int.MaxValue, book.Pages);
            Assert.Equal(decimal.MaxValue, book.BaseCost);
        }

        /// <summary>
        /// Проверяет, что Price может быть изменён независимо от BaseCost.
        /// Это важно для системы ценообразования.
        /// </summary>
        [Fact]
        public void Price_CanBeSetIndependentlyFromBaseCost()
        {
            // Arrange - Создаём книгу с известной себестоимостью
            var book = new Book(1, "Книга", "Автор", "Жанр", 200, 500m);

            // Act - Изменяем цену продажи
            book.Price = 1000m;

            // Assert - Себестоимость не изменилась, цена изменилась
            Assert.Equal(500m, book.BaseCost);
            Assert.Equal(1000m, book.Price);
        }

        /// <summary>
        /// Проверяет, что Price может быть меньше BaseCost (скидка).
        /// Это допустимая ситуация, хоть и не типичная.
        /// </summary>
        [Fact]
        public void Price_CanBeLessThanBaseCost()
        {
            // Arrange - Создаём книгу
            var book = new Book(1, "Книга", "Автор", "Жанр", 200, 500m);

            // Act - Устанавливаем цену ниже себестоимости
            book.Price = 300m;

            // Assert - Цена установлена корректно
            Assert.Equal(300m, book.Price);
            Assert.True(book.Price < book.BaseCost);
        }

        #endregion
    }
}
