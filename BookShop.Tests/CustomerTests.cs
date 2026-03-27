using BookShopLibrary;
using Xunit;

namespace BookShop.Tests
{
    /// <summary>
    /// Класс тестов для проверки функциональности класса Customer.
    /// Содержит тесты конструкторов для разных типов клиентов,
    /// генерации случайных покупателей и строкового представления.
    /// Проверяет логику поиска по жанру и конкретной книге.
    /// </summary>
    public class CustomerTests
    {
        #region Конструктор по жанру

        /// <summary>
        /// Проверяет, что конструктор по жанру создаёт клиента,
        /// который ищет любую книгу указанного жанра.
        /// WantsGenreOnly должно быть true.
        /// </summary>
        [Fact]
        public void Constructor_ByGenre_ShouldCreateGenreCustomer()
        {
            // Arrange & Act - Создаём клиента по жанру
            var customer = new Customer("Фантастика");

            // Assert - Клиент ищет по жанру
            Assert.True(customer.WantsGenreOnly);        // Флаг жанра = true
            Assert.Equal("Фантастика", customer.DesiredGenre); // Жанр сохранён
        }

        /// <summary>
        /// Проверяет, что для клиента по жанру свойства DesiredTitle и DesiredAuthor
        /// имеют значения по умолчанию (null или пустые).
        /// </summary>
        [Fact]
        public void Constructor_ByGenre_ShouldNotSetTitleAndAuthor()
        {
            // Arrange & Act - Создаём клиента по жанру
            var customer = new Customer("Детектив");

            // Assert - Название и автор не заданы
            Assert.True(customer.WantsGenreOnly);
            Assert.Null(customer.DesiredTitle);
            Assert.Null(customer.DesiredAuthor);
        }

        /// <summary>
        /// Проверяет создание клиента с разными жанрами.
        /// </summary>
        [Theory]
        [InlineData("Фантастика")]
        [InlineData("Детектив")]
        [InlineData("Роман")]
        [InlineData("Классика")]
        public void Constructor_ByGenre_ShouldAcceptDifferentGenres(string genre)
        {
            // Arrange & Act - Создаём клиента с указанным жанром
            var customer = new Customer(genre);

            // Assert - Жанр сохранён корректно
            Assert.Equal(genre, customer.DesiredGenre);
            Assert.True(customer.WantsGenreOnly);
        }

        #endregion

        #region Конструктор по книге

        /// <summary>
        /// Проверяет, что конструктор по книге создаёт клиента,
        /// который ищет конкретную книгу по названию и автору.
        /// WantsGenreOnly должно быть false.
        /// </summary>
        [Fact]
        public void Constructor_ByBook_ShouldCreateBookCustomer()
        {
            // Arrange & Act - Создаём клиента по книге
            var customer = new Customer("Война и мир", "Толстой");

            // Assert - Клиент ищет конкретную книгу
            Assert.False(customer.WantsGenreOnly);            // Флаг жанра = false
            Assert.Equal("Война и мир", customer.DesiredTitle);  // Название сохранено
            Assert.Equal("Толстой", customer.DesiredAuthor);     // Автор сохранён
        }

        /// <summary>
        /// Проверяет, что для клиента по книге свойство DesiredGenre
        /// имеет значение по умолчанию (null).
        /// </summary>
        [Fact]
        public void Constructor_ByBook_ShouldNotSetGenre()
        {
            // Arrange & Act - Создаём клиента по книге
            var customer = new Customer("Дюна", "Герберт");

            // Assert - Жанр не задан
            Assert.False(customer.WantsGenreOnly);
            Assert.Null(customer.DesiredGenre);
        }

        /// <summary>
        /// Проверяет создание клиента с разными комбинациями книга-автор.
        /// </summary>
        [Theory]
        [InlineData("Война и мир", "Толстой")]
        [InlineData("Дюна", "Герберт")]
        [InlineData("1984", "Оруэлл")]
        public void Constructor_ByBook_ShouldAcceptDifferentBooks(string title, string author)
        {
            // Arrange & Act - Создаём клиента с указанной книгой
            var customer = new Customer(title, author);

            // Assert - Книга и автор сохранены
            Assert.Equal(title, customer.DesiredTitle);
            Assert.Equal(author, customer.DesiredAuthor);
            Assert.False(customer.WantsGenreOnly);
        }

        #endregion

        #region Метод ToString

        /// <summary>
        /// Проверяет строковое представление клиента по жанру.
        /// Формат: "Хочу любую книгу жанра: {Жанр}"
        /// </summary>
        [Fact]
        public void ToString_ShouldReturnCorrectString_ForGenreCustomer()
        {
            // Arrange - Создаём клиента по жанру
            var customer = new Customer("Фантастика");

            // Act - Получаем строковое представление
            var result = customer.ToString();

            // Assert - Формат для жанра
            Assert.Equal("Хочу любую книгу жанра: Фантастика", result);
        }

        /// <summary>
        /// Проверяет строковое представление клиента по конкретной книге.
        /// Формат: "Ищу книгу: «{Название}», автор: {Автор}"
        /// </summary>
        [Fact]
        public void ToString_ShouldReturnCorrectString_ForBookCustomer()
        {
            // Arrange - Создаём клиента по книге
            var customer = new Customer("Дюна", "Герберт");

            // Act - Получаем строковое представление
            var result = customer.ToString();

            // Assert - Формат для книги
            Assert.Equal("Ищу книгу: «Дюна», автор: Герберт", result);
        }

        /// <summary>
        /// Проверяет, что ToString правильно отображает разные жанры.
        /// </summary>
        [Theory]
        [InlineData("Фантастика", "Хочу любую книгу жанра: Фантастика")]
        [InlineData("Детектив", "Хочу любую книгу жанра: Детектив")]
        public void ToString_ShouldFormatGenreCorrectly(string genre, string expected)
        {
            // Arrange - Создаём клиента по жанру
            var customer = new Customer(genre);

            // Act - Получаем строку
            var result = customer.ToString();

            // Assert - Ожидаемый формат
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Проверяет, что ToString правильно отображает разные книги.
        /// </summary>
        [Theory]
        [InlineData("Война и мир", "Толстой", "Ищу книгу: «Война и мир», автор: Толстой")]
        [InlineData("1984", "Оруэлл", "Ищу книгу: «1984», автор: Оруэлл")]
        public void ToString_ShouldFormatBookCorrectly(string title, string author, string expected)
        {
            // Arrange - Создаём клиента по книге
            var customer = new Customer(title, author);

            // Act - Получаем строку
            var result = customer.ToString();

            // Assert - Ожидаемый формат
            Assert.Equal(expected, result);
        }

        #endregion

        #region Граничные случаи

        /// <summary>
        /// Проверяет создание клиента с пустой строкой жанра.
        /// Это допустимый случай (клиент не определился с жанром).
        /// </summary>
        [Fact]
        public void Constructor_ByGenre_ShouldAcceptEmptyGenre()
        {
            // Arrange & Act - Создаём клиента с пустым жанром
            var customer = new Customer("");

            // Assert - Жанр пустой, но клиент создан
            Assert.Equal("", customer.DesiredGenre);
            Assert.True(customer.WantsGenreOnly);
        }

        /// <summary>
        /// Проверяет создание клиента с пустыми названием и автором.
        /// Это граничный случай для тестирования.
        /// </summary>
        [Fact]
        public void Constructor_ByBook_ShouldAcceptEmptyStrings()
        {
            // Arrange & Act - Создаём клиента с пустыми данными
            var customer = new Customer("", "");

            // Assert - Данные пустые, но клиент создан
            Assert.Equal("", customer.DesiredTitle);
            Assert.Equal("", customer.DesiredAuthor);
            Assert.False(customer.WantsGenreOnly);
        }

        /// <summary>
        /// Проверяет создание клиента с длинным названием книги.
        /// </summary>
        [Fact]
        public void Constructor_ByBook_ShouldAcceptLongTitle()
        {
            // Arrange - Создаём длинное название
            var longTitle = new string('А', 1000);

            // Act - Создаём клиента
            var customer = new Customer(longTitle, "Автор");

            // Assert - Название сохранено полностью
            Assert.Equal(longTitle, customer.DesiredTitle);
            Assert.Equal(1000, customer.DesiredTitle.Length);
        }

        #endregion
    }
}
