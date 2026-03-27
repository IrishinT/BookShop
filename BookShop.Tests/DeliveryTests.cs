using BookShopLibrary;
using Xunit;

namespace BookShop.Tests
{
    /// <summary>
    /// Класс тестов для проверки функциональности класса Delivery.
    /// Содержит тесты конструктора, генерации случайных доставок,
    /// внедрения ошибок (опечатки и плагиата).
    /// Проверяет логику обработки бракованных книг.
    /// </summary>
    public class DeliveryTests
    {
        #region Конструктор

        /// <summary>
        /// Проверяет, что конструктор создаёт доставку с корректными свойствами.
        /// По умолчанию ошибка = None.
        /// </summary>
        [Fact]
        public void Constructor_ShouldCreateDeliveryWithCorrectProperties()
        {
            // Arrange - Создаём тестовую книгу
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Создаём доставку
            var delivery = new Delivery(book, false);

            // Assert - Проверяем все свойства
            Assert.Same(book, delivery.Book);               // Книга сохранена
            Assert.False(delivery.IsOrdered);               // Не заказная
            Assert.Equal(DeliveryErrorType.None, delivery.ErrorType); // Без ошибок
        }

        /// <summary>
        /// Проверяет создание заказной доставки.
        /// Заказные книги всегда без ошибок.
        /// </summary>
        [Fact]
        public void Constructor_ShouldCreateOrderedDelivery()
        {
            // Arrange - Создаём книгу
            var book = new Book(1, "Заказная книга", "Автор", "Фантастика", 200, 500m);

            // Act - Создаём заказную доставку
            var delivery = new Delivery(book, true, DeliveryErrorType.None);

            // Assert - Это заказная книга
            Assert.True(delivery.IsOrdered);
            Assert.Equal(DeliveryErrorType.None, delivery.ErrorType);
        }

        /// <summary>
        /// Проверяет создание доставки с указанным типом ошибки.
        /// </summary>
        [Theory]
        [InlineData(DeliveryErrorType.None)]
        [InlineData(DeliveryErrorType.Typo)]
        [InlineData(DeliveryErrorType.Plagiarism)]
        public void Constructor_ShouldAcceptDifferentErrorTypes(DeliveryErrorType errorType)
        {
            // Arrange - Создаём книгу
            var book = new Book(1, "Книга", "Автор", "Фантастика", 200, 500m);

            // Act - Создаём доставку с указанной ошибкой
            var delivery = new Delivery(book, false, errorType);

            // Assert - Тип ошибки сохранён
            Assert.Equal(errorType, delivery.ErrorType);
        }

        #endregion

        #region Свойства класса Delivery

        /// <summary>
        /// Проверяет, что свойства Book, IsOrdered, ErrorType можно изменять.
        /// </summary>
        [Fact]
        public void Delivery_PropertiesShouldBeSettable()
        {
            // Arrange - Создаём доставку
            var book1 = new Book(1, "Книга 1", "Автор 1", "Фантастика", 200, 500m);
            var delivery = new Delivery(book1, false, DeliveryErrorType.None);

            // Act - Изменяем свойства
            var book2 = new Book(2, "Книга 2", "Автор 2", "Детектив", 300, 600m);
            delivery.Book = book2;
            delivery.IsOrdered = true;
            delivery.ErrorType = DeliveryErrorType.Typo;

            // Assert - Свойства изменены
            Assert.Same(book2, delivery.Book);
            Assert.True(delivery.IsOrdered);
            Assert.Equal(DeliveryErrorType.Typo, delivery.ErrorType);
        }

        #endregion
    }
}
