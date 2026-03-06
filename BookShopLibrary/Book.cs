using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace BookShopLibrary
{
    /// <summary>
    /// Класс, представляющий книгу в книжном магазине
    /// </summary>
    public class Book
    {
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
        /// Конструктор для создания книги
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
        /// Переопределение ToString для отображения в списках
        /// </summary>
        public override string ToString()
        {
            return $"{Id}: {Title} - {Author}";
        }
    }
}