using BookShopLibrary;
using Xunit;

namespace BookShop.Tests
{
    public class BookTests
    {
        [Fact]
        public void Constructor_ShouldCreateBookWithCorrectProperties()
        {
            // Arrange & Act
            var book = new Book(1, "Тестовая книга", "Тестовый автор", "Фантастика", 300, 500m);

            // Assert
            Assert.Equal(1, book.Id);
            Assert.Equal("Тестовая книга", book.Title);
            Assert.Equal("Тестовый автор", book.Author);
            Assert.Equal("Фантастика", book.Genre);
            Assert.Equal(300, book.Pages);
            Assert.Equal(500m, book.Price);
        }

        [Fact]
        public void Sell_ShouldReturnBookPrice()
        {
            // Arrange
            var book = new Book(1, "Книга", "Автор", "Жанр", 200, 1000m);

            // Act
            var price = book.Sell();

            // Assert
            Assert.Equal(1000m, price);
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var book = new Book(5, "Война и мир", "Толстой", "Классика", 1200, 1500m);

            // Act
            var result = book.ToString();

            // Assert
            Assert.Equal("5: Война и мир - Толстой", result);
        }

        [Fact]
        public void GenerateRandom_ShouldThrowException_WhenFilesNotFound()
        {
            // Act & Assert
            var exception = Assert.Throws<Exception>(() => Book.GenerateRandom(1));
            Assert.Contains("Ошибка при генерации книги", exception.Message);
        }

        [Fact]
        public void GenerateRandom_ShouldGenerateBook_WhenFilesExist()
        {
            // Arrange
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(basePath);
            File.WriteAllLines(Path.Combine(basePath, "titles.txt"), new[] { "Тестовая книга" });
            File.WriteAllLines(Path.Combine(basePath, "authors.txt"), new[] { "Тестовый автор" });
            File.WriteAllLines(Path.Combine(basePath, "genres.txt"), new[] { "Тестовый жанр" });

            try
            {
                // Act
                var book = Book.GenerateRandom(100);

                // Assert
                Assert.NotNull(book);
                Assert.Equal(100, book.Id);
                Assert.Equal("Тестовая книга", book.Title);
                Assert.Equal("Тестовый автор", book.Author);
                Assert.Equal("Тестовый жанр", book.Genre);
                Assert.InRange(book.Pages, 50, 2000);
                Assert.InRange(book.Price, 100m, 5000m);
            }
            finally
            {
                // Cleanup
                File.Delete(Path.Combine(basePath, "titles.txt"));
                File.Delete(Path.Combine(basePath, "authors.txt"));
                File.Delete(Path.Combine(basePath, "genres.txt"));
            }
        }
    }
}
