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
        /// Конструктор для создания книги с указанным ID
        /// </summary>
        /// <param name="id">Идентификационный номер</param>
        /// <param name="title">Название</param>
        /// <param name="author">Автор</param>
        /// <param name="genre">Жанр</param>
        /// <param name="pages">Количество страниц</param>
        /// <param name="price">Цена</param>
        public Book(int id, string title, string author, string genre, int pages, decimal price)
        {
            Id = id;
            Title = title;
            Author = author;
            Genre = genre;
            Pages = pages;
            Price = price;
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
        /// <returns>Случайно сгенерированная книга</returns>
        public static Book GenerateRandom(int nextId)
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                // Чтение данных из файлов
                string[] titles = File.ReadAllLines(Path.Combine(basePath, "titles.txt"), Encoding.UTF8);
                string[] authors = File.ReadAllLines(Path.Combine(basePath, "authors.txt"), Encoding.UTF8);
                string[] genres = File.ReadAllLines(Path.Combine(basePath, "genres.txt"), Encoding.UTF8);

                // Генерация названия
                string title = titles[random.Next(titles.Length)].Trim();

                // Генерация автора
                string author = authors[random.Next(authors.Length)].Trim();

                // Генерация жанра
                string genre = genres[random.Next(genres.Length)].Trim();

                // Генерация случайных чисел
                int pages = random.Next(50, 2001);
                decimal price = random.Next(100, 5001);

                return new Book(nextId, title, author, genre, pages, price);
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
            return $"{Id}: {Title} - {Author}";
        }
    }
}