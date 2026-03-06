using System;
using System.Collections.Generic;
using System.Linq;
using BookShopLibrary;

namespace BookShopLibrary
{
    /// <summary>
    /// Класс, представляющий книжный шкаф.
    /// В одном шкафу могут храниться книги только одного жанра.
    /// </summary>
    public class BookShelf
    {
        private List<Book> books = new List<Book>();

        /// <summary>
        /// Уникальный идентификатор шкафа
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Жанр книг в шкафу
        /// </summary>
        public string Genre { get; private set; }

        /// <summary>
        /// Вместимость шкафа (максимальное количество книг)
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Текущее количество книг в шкафу
        /// </summary>
        public int CurrentCount => books.Count;

        /// <summary>
        /// Конструктор книжного шкафа
        /// </summary>
        /// <param name="id">Идентификатор шкафа</param>
        /// <param name="genre">Жанр книг</param>
        /// <param name="capacity">Вместимость</param>
        public BookShelf(int id, string genre, int capacity)
        {
            Id = id;
            Genre = genre;
            Capacity = capacity;
        }

        /// <summary>
        /// Добавление книги в шкаф
        /// </summary>
        /// <param name="book">Добавляемая книга</param>
        /// <returns>true - книга добавлена, false - нет места</returns>
        /// <exception cref="InvalidOperationException">Если жанр книги не соответствует жанру шкафа</exception>
        public bool AddBook(Book book)
        {
            if (book.Genre != Genre)
                throw new InvalidOperationException($"Жанр книги '{book.Genre}' не соответствует жанру шкафа '{Genre}'");

            if (books.Count >= Capacity)
                return false;

            books.Add(book);
            return true;
        }

        /// <summary>
        /// Удаление книги из шкафа по ID
        /// </summary>
        /// <param name="bookId">ID книги</param>
        /// <returns>true - книга удалена, false - книга не найдена</returns>
        public bool RemoveBook(int bookId)
        {
            var book = FindBookById(bookId);
            if (book != null)
            {
                return books.Remove(book);
            }
            return false;
        }

        /// <summary>
        /// Поиск книги по названию
        /// </summary>
        /// <param name="title">Название книги</param>
        /// <returns>Найденная книга или null</returns>
        public Book FindBookByTitle(string title)
        {
            return books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Поиск книги по идентификационному номеру
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <returns>Найденная книга или null</returns>
        public Book FindBookById(int id)
        {
            return books.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Получение всех книг в шкафу
        /// </summary>
        /// <returns>Список книг</returns>
        public List<Book> GetAllBooks()
        {
            return books.ToList();
        }

        /// <summary>
        /// Изменение жанра шкафа (только для пустого шкафа)
        /// </summary>
        /// <param name="newGenre">Новый жанр</param>
        /// <exception cref="InvalidOperationException">Если шкаф не пуст</exception>
        public void ChangeGenre(string newGenre)
        {
            if (books.Count > 0)
                throw new InvalidOperationException("Нельзя изменить жанр непустого шкафа");

            Genre = newGenre;
        }
    }
}