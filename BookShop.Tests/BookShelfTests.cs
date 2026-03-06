using BookShopLibrary;
using Xunit;

namespace BookShop.Tests
{
    public class BookShelfTests
    {
        [Fact]
        public void Constructor_ShouldCreateBookShelfWithCorrectProperties()
        {
            // Arrange & Act
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Assert
            Assert.Equal(1, shelf.Id);
            Assert.Equal("Фантастика", shelf.Genre);
            Assert.Equal(10, shelf.Capacity);
            Assert.Equal(0, shelf.CurrentCount);
            Assert.True(shelf.IsEmpty);
            Assert.True(shelf.HasFreeSpace);
        }

        [Fact]
        public void AddBook_ShouldAddBookSuccessfully()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act
            var result = shelf.AddBook(book);

            // Assert
            Assert.True(result);
            Assert.Equal(1, shelf.CurrentCount);
            Assert.False(shelf.IsEmpty);
        }

        [Fact]
        public void AddBook_ShouldThrowException_WhenGenreMismatch()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Детектив", 200, 500m);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => shelf.AddBook(book));
        }

        [Fact]
        public void AddBook_ShouldReturnFalse_WhenShelfIsFull()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 2);
            var book1 = new Book(1, "Книга 1", "Автор", "Фантастика", 200, 500m);
            var book2 = new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m);
            var book3 = new Book(3, "Книга 3", "Автор", "Фантастика", 200, 500m);

            // Act
            shelf.AddBook(book1);
            shelf.AddBook(book2);
            var result = shelf.AddBook(book3);

            // Assert
            Assert.False(result);
            Assert.Equal(2, shelf.CurrentCount);
        }

        [Fact]
        public void RemoveBook_ShouldRemoveBookSuccessfully()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);

            // Act
            var result = shelf.RemoveBook(1);

            // Assert
            Assert.True(result);
            Assert.Equal(0, shelf.CurrentCount);
        }

        [Fact]
        public void FindBookByTitle_ShouldReturnBook_WhenExists()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Война миров", "Уэллс", "Фантастика", 300, 700m);
            shelf.AddBook(book);

            // Act
            var found = shelf.FindBookByTitle("Война миров");

            // Assert
            Assert.NotNull(found);
            Assert.Equal("Война миров", found.Title);
        }

        [Fact]
        public void FindBookById_ShouldReturnBook_WhenExists()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(5, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);

            // Act
            var found = shelf.FindBookById(5);

            // Assert
            Assert.NotNull(found);
            Assert.Equal(5, found.Id);
        }

        [Fact]
        public void SellBook_ShouldRemoveBookAndReturnPrice()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 1000m);
            shelf.AddBook(book);

            // Act
            var price = shelf.SellBook(1);

            // Assert
            Assert.Equal(1000m, price);
            Assert.Equal(0, shelf.CurrentCount);
        }

        [Fact]
        public void SellBook_ShouldThrowException_WhenBookNotFound()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => shelf.SellBook(999));
        }

        [Fact]
        public void ChangeGenre_ShouldChangeGenre_WhenShelfIsEmpty()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act
            shelf.ChangeGenre("Детектив");

            // Assert
            Assert.Equal("Детектив", shelf.Genre);
        }

        [Fact]
        public void ChangeGenre_ShouldThrowException_WhenShelfIsNotEmpty()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => shelf.ChangeGenre("Детектив"));
        }

        [Fact]
        public void AddBook_ShouldThrowException_WhenBookIsNull()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => shelf.AddBook(null));
        }

        [Fact]
        public void RemoveBook_ShouldReturnFalse_WhenBookNotFound()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act
            var result = shelf.RemoveBook(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void FindBookByTitle_ShouldReturnNull_WhenTitleIsEmpty()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act
            var result = shelf.FindBookByTitle("");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAllBooks_ShouldReturnListOfBooks()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book1 = new Book(1, "Книга 1", "Автор", "Фантастика", 200, 500m);
            var book2 = new Book(2, "Книга 2", "Автор", "Фантастика", 300, 600m);
            shelf.AddBook(book1);
            shelf.AddBook(book2);

            // Act
            var books = shelf.GetAllBooks();

            // Assert
            Assert.Equal(2, books.Count);
        }

        [Fact]
        public void ChangeGenre_ShouldThrowException_WhenGenreIsEmpty()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => shelf.ChangeGenre(""));
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);

            // Act
            var result = shelf.ToString();

            // Assert
            Assert.Equal("Шкаф 1 (Фантастика) - 1/10", result);
        }
    }
}
