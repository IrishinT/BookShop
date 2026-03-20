using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BookShopLibrary
{
    /// <summary>
    /// Класс, представляющий книгу в книжном магазине
    /// </summary>
    public class Book
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Название книги
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Автор книги
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Уникальный идентификационный номер
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Жанр книги
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Количество страниц
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Цена книги
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Номер части книги (0 - оригинал, 1,2,... - продолжения)
        /// </summary>
        public int PartNumber { get; set; } = 0;

        /// <summary>
        /// Отображаемое название с учётом номера части
        /// </summary>
        public string DisplayTitle => PartNumber == 0 ? Title : $"{Title} {PartNumber}";

        /// <summary>
        /// Конструктор для создания книги с указанным ID
        /// </summary>
        /// <param name="id">Идентификационный номер</param>
        /// <param name="title">Название</param>
        /// <param name="author">Автор</param>
        /// <param name="genre">Жанр</param>
        /// <param name="pages">Количество страниц</param>
        /// <param name="price">Цена</param>
        /// <param name="partNumber">Номер части (по умолчанию 0)</param>
        public Book(int id, string title, string author, string genre, int pages, decimal price, int partNumber = 0)
        {
            Id = id;
            Title = title;
            Author = author;
            Genre = genre;
            Pages = pages;
            Price = price;
            PartNumber = partNumber;
        }

        /// <summary>
        /// Метод продажи книги
        /// </summary>
        /// <returns>Цена книги</returns>
        public decimal Sell()
        {
            return Price;
        }

        /// <summary>
        /// Статический метод случайной генерации книги
        /// </summary>
        /// <param name="nextId">Следующий ID для новой книги</param>
        /// <param name="bookAuthorPairs">Список пар "название;автор"</param>
        /// <param name="genres">Список жанров</param>
        /// <returns>Случайно сгенерированная книга</returns>
        public static Book GenerateRandom(int nextId, List<(string Title, string Author)> bookAuthorPairs, List<string> genres)
        {
            try
            {
                // Выбираем случайную пару "название-автор"
                var pair = bookAuthorPairs[random.Next(bookAuthorPairs.Count)];
                // Выбираем случайный жанр
                string genre = genres[random.Next(genres.Count)];

                // Генерация случайных чисел
                int pages = random.Next(50, 2001);
                decimal price = random.Next(100, 5001);

                return new Book(nextId, pair.Title, pair.Author, genre, pages, price);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при генерации книги: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Переопределение ToString для отображения в списках
        /// </summary>
        public override string ToString()
        {
            return $"{Id}: {DisplayTitle} - {Author}";
        }
    }
}
        {
            return $"{Id}: {Title} - {Author}";
        }
    }
}
