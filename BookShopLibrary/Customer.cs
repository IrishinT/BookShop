using System;

namespace BookShopLibrary
{
    public class Customer
    {
        private static readonly Random random = new Random();

        public bool WantsGenreOnly { get; private set; }
        public string DesiredGenre { get; private set; }
        public string DesiredTitle { get; private set; }
        public string DesiredAuthor { get; private set; }

        public Customer(string genre)
        {
            WantsGenreOnly = true; DesiredGenre = genre;
        }

        public Customer(string title, string author)
        {
            WantsGenreOnly = false; DesiredTitle = title; DesiredAuthor = author;
        }

        public static Customer GenerateRandomCustomer()
        {
            if (random.Next(2) == 0)
                return new Customer(DatabaseManager.Genres[random.Next(DatabaseManager.Genres.Count)]);
            
            var pair = DatabaseManager.BookAuthorPairs[random.Next(DatabaseManager.BookAuthorPairs.Count)];
            return new Customer(pair.Title, pair.Author);
        }

        public override string ToString() => WantsGenreOnly 
            ? $"Хочу любую книгу жанра: {DesiredGenre}" 
            : $"Ищу книгу: «{DesiredTitle}», автор: {DesiredAuthor}";
    }
}