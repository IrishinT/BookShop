using BookShopLibrary;
using Xunit;

namespace BookShop.Tests
{
    public class ShopTests
    {
        [Fact]
        public void Constructor_ShouldCreateShopWithCorrectProperties()
        {
            // Arrange & Act
            var shop = new Shop(1000m, 5);

            // Assert
            Assert.Equal(1000m, shop.Balance);
            Assert.Equal(5, shop.MaxShelves);
            Assert.Equal(0, shop.ShelfCount);
            Assert.True(shop.CanAddShelf);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenMaxShelvesIsZeroOrNegative()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Shop(1000m, 0));
            Assert.Throws<ArgumentException>(() => new Shop(1000m, -1));
        }

        [Fact]
        public void AddBookShelf_ShouldAddShelfSuccessfully()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);

            // Act
            shop.AddBookShelf(shelf);

            // Assert
            Assert.Equal(1, shop.ShelfCount);
        }

        [Fact]
        public void AddBookShelf_ShouldThrowException_WhenMaxShelvesReached()
        {
            // Arrange
            var shop = new Shop(1000m, 2);
            var shelf1 = new BookShelf(1, "Фантастика", 10);
            var shelf2 = new BookShelf(2, "Детектив", 10);
            var shelf3 = new BookShelf(3, "Роман", 10);
            shop.AddBookShelf(shelf1);
            shop.AddBookShelf(shelf2);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => shop.AddBookShelf(shelf3));
        }

        [Fact]
        public void CreateBookShelf_ShouldCreateAndAddShelf()
        {
            // Arrange
            var shop = new Shop(1000m, 5);

            // Act
            var shelf = shop.CreateBookShelf(1, "Фантастика", 10);

            // Assert
            Assert.NotNull(shelf);
            Assert.Equal(1, shop.ShelfCount);
            Assert.Equal("Фантастика", shelf.Genre);
        }

        [Fact]
        public void RemoveBookShelf_ShouldRemoveEmptyShelf()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);

            // Act
            var result = shop.RemoveBookShelf(1);

            // Assert
            Assert.True(result);
            Assert.Equal(0, shop.ShelfCount);
        }

        [Fact]
        public void RemoveBookShelf_ShouldThrowException_WhenShelfIsNotEmpty()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);
            shop.AddBookShelf(shelf);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => shop.RemoveBookShelf(1));
        }

        [Fact]
        public void AddToBalance_ShouldIncreaseBalance()
        {
            // Arrange
            var shop = new Shop(1000m, 5);

            // Act
            shop.AddToBalance(500m);

            // Assert
            Assert.Equal(1500m, shop.Balance);
        }

        [Fact]
        public void AddToBalance_ShouldThrowException_WhenAmountIsNegative()
        {
            // Arrange
            var shop = new Shop(1000m, 5);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => shop.AddToBalance(-100m));
        }

        [Fact]
        public void FindBookByTitle_ShouldReturnBook_WhenExists()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(1, "Дюна", "Герберт", "Фантастика", 600, 1200m);
            shelf.AddBook(book);
            shop.AddBookShelf(shelf);

            // Act
            var found = shop.FindBookByTitle("Дюна");

            // Assert
            Assert.NotNull(found);
            Assert.Equal("Дюна", found.Title);
        }

        [Fact]
        public void FindBookById_ShouldReturnBook_WhenExists()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            var book = new Book(10, "Книга", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book);
            shop.AddBookShelf(shelf);

            // Act
            var found = shop.FindBookById(10);

            // Assert
            Assert.NotNull(found);
            Assert.Equal(10, found.Id);
        }

        [Fact]
        public void TryAddBook_ShouldAddBookToExistingShelf()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act
            var result = shop.TryAddBook(book);

            // Assert
            Assert.True(result);
            Assert.Equal(1, shop.TotalBooksCount);
        }

        [Fact]
        public void TryAddBook_ShouldCreateNewShelf_WhenNoSuitableShelfExists()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act
            var result = shop.TryAddBook(book);

            // Assert
            Assert.True(result);
            Assert.Equal(1, shop.ShelfCount);
            Assert.Equal(1, shop.TotalBooksCount);
        }

        [Fact]
        public void TryAddBook_ShouldReuseEmptyShelf()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Детектив", 10);
            shop.AddBookShelf(shelf);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act
            var result = shop.TryAddBook(book);

            // Assert
            Assert.True(result);
            Assert.Equal(1, shop.ShelfCount);
            Assert.Equal("Фантастика", shelf.Genre);
        }

        [Fact]
        public void TryAddBook_ShouldReturnFalse_WhenNoSpaceAvailable()
        {
            // Arrange
            var shop = new Shop(1000m, 1);
            var shelf = new BookShelf(1, "Детектив", 1);
            var book1 = new Book(1, "Книга 1", "Автор", "Детектив", 200, 500m);
            var book2 = new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m);
            shelf.AddBook(book1);
            shop.AddBookShelf(shelf);

            // Act
            var result = shop.TryAddBook(book2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddBookShelf_ShouldThrowException_WhenShelfIsNull()
        {
            // Arrange
            var shop = new Shop(1000m, 5);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => shop.AddBookShelf(null));
        }

        [Fact]
        public void RemoveBookShelf_ShouldReturnFalse_WhenShelfNotFound()
        {
            // Arrange
            var shop = new Shop(1000m, 5);

            // Act
            var result = shop.RemoveBookShelf(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void FindBookByTitle_ShouldReturnNull_WhenTitleIsEmpty()
        {
            // Arrange
            var shop = new Shop(1000m, 5);

            // Act
            var result = shop.FindBookByTitle("");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void FindBookByTitle_ShouldReturnNull_WhenBookNotFound()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);

            // Act
            var result = shop.FindBookByTitle("Несуществующая книга");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void FindBookById_ShouldReturnNull_WhenBookNotFound()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);

            // Act
            var result = shop.FindBookById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAllBooks_ShouldReturnDictionaryOfBooks()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf1 = new BookShelf(1, "Фантастика", 10);
            var shelf2 = new BookShelf(2, "Детектив", 10);
            var book1 = new Book(1, "Книга 1", "Автор", "Фантастика", 200, 500m);
            var book2 = new Book(2, "Книга 2", "Автор", "Детектив", 200, 500m);
            shelf1.AddBook(book1);
            shelf2.AddBook(book2);
            shop.AddBookShelf(shelf1);
            shop.AddBookShelf(shelf2);

            // Act
            var allBooks = shop.GetAllBooks();

            // Assert
            Assert.Equal(2, allBooks.Count);
            Assert.Single(allBooks[shelf1]);
            Assert.Single(allBooks[shelf2]);
        }

        [Fact]
        public void GetShelves_ShouldReturnListOfShelves()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf1 = new BookShelf(1, "Фантастика", 10);
            var shelf2 = new BookShelf(2, "Детектив", 10);
            shop.AddBookShelf(shelf1);
            shop.AddBookShelf(shelf2);

            // Act
            var shelves = shop.GetShelves();

            // Assert
            Assert.Equal(2, shelves.Count);
        }

        [Fact]
        public void Shelves_ShouldReturnReadOnlyList()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf = new BookShelf(1, "Фантастика", 10);
            shop.AddBookShelf(shelf);

            // Act
            var shelves = shop.Shelves;

            // Assert
            Assert.Single(shelves);
        }

        [Fact]
        public void TryAddBook_ShouldThrowException_WhenBookIsNull()
        {
            // Arrange
            var shop = new Shop(1000m, 5);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => shop.TryAddBook(null));
        }

        [Fact]
        public void TryAddBook_ShouldCreateShelfWithId1_WhenNoShelvesExist()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act
            var result = shop.TryAddBook(book);

            // Assert
            Assert.True(result);
            Assert.Equal(1, shop.ShelfCount);
            Assert.Equal(1, shop.Shelves[0].Id);
        }

        [Fact]
        public void TryAddBook_ShouldCreateShelfWithIncrementedId_WhenShelvesExist()
        {
            // Arrange
            var shop = new Shop(1000m, 5);
            var shelf1 = new BookShelf(3, "Детектив", 1);
            var book1 = new Book(1, "Книга 1", "Автор", "Детектив", 200, 500m);
            shelf1.AddBook(book1); // Заполняем первый шкаф
            shop.AddBookShelf(shelf1);
            
            var book2 = new Book(2, "Книга 2", "Автор", "Фантастика", 200, 500m);

            // Act
            var result = shop.TryAddBook(book2);

            // Assert
            Assert.True(result);
            Assert.Equal(2, shop.ShelfCount);
            Assert.Equal(4, shop.Shelves[1].Id); // ID должен быть Max(3) + 1 = 4
        }
    }
}
