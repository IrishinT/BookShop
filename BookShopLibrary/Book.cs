using System;

namespace BookShopLibrary
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Pages { get; set; }
        
        /// <summary> Цена продажи (вводит игрок) </summary>
        public decimal Price { get; set; }

        /// <summary> Базовая цена (себестоимость). По ней происходит заказ и списываются деньги. </summary>
        public decimal BaseCost { get; set; }

        /// <summary> Номер части (сиквел). 0 - оригинал, 1 - вторая часть и т.д. </summary>
        public int PartNumber { get; set; }

        /// <summary> Отображаемое название (сиквел: "Название 2") </summary>
        public string DisplayTitle => PartNumber == 0 ? Title : $"{Title} {PartNumber + 1}";

        public Book(int id, string title, string author, string genre, int pages, decimal baseCost, int partNumber = 0)
        {
            Id = id;
            Title = title;
            Author = author;
            Genre = genre;
            Pages = pages;
            BaseCost = baseCost;
            Price = baseCost;
            PartNumber = partNumber;
        }

        public Book Clone() => new Book(Id, Title, Author, Genre, Pages, BaseCost, PartNumber);

        /// <summary>
        /// Продажа книги
        /// </summary>
        /// <returns>Цена продажи книги</returns>
        public decimal Sell()
        {
            return Price;
        }

        /// <summary>
        /// Генерация случайной книги из файлов
        /// </summary>
        public static Book GenerateRandom(int id)
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var titles = System.IO.File.ReadAllLines(System.IO.Path.Combine(basePath, "titles.txt"));
                var authors = System.IO.File.ReadAllLines(System.IO.Path.Combine(basePath, "authors.txt"));
                var genres = System.IO.File.ReadAllLines(System.IO.Path.Combine(basePath, "genres.txt"));

                var random = new Random();
                string title = titles[random.Next(titles.Length)];
                string author = authors[random.Next(authors.Length)];
                string genre = genres[random.Next(genres.Length)];
                int pages = random.Next(100, 1000);
                decimal baseCost = random.Next(100, 1000);

                return new Book(id, title, author, genre, pages, baseCost);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при генерации книги: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Генерация случайной книги из переданных данных
        /// </summary>
        public static Book GenerateRandom(int id, List<(string Title, string Author)> bookAuthorPairs, List<string> genres)
        {
            var random = new Random();
            
            if (bookAuthorPairs == null || bookAuthorPairs.Count == 0)
                throw new ArgumentException("Список пар книга-автор не может быть пустым", nameof(bookAuthorPairs));
            
            if (genres == null || genres.Count == 0)
                throw new ArgumentException("Список жанров не может быть пустым", nameof(genres));

            var pair = bookAuthorPairs[random.Next(bookAuthorPairs.Count)];
            string title = pair.Title;
            string author = pair.Author;
            string genre = genres[random.Next(genres.Count)];
            int pages = random.Next(100, 1000);
            decimal baseCost = random.Next(100, 1000);

            return new Book(id, title, author, genre, pages, baseCost);
        }

        public override string ToString() => $"{Id}: {DisplayTitle} - {Author} ({Genre}) | Себестоимость: {BaseCost:C}";
    }
}