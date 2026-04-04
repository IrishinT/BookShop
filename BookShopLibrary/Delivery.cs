using System;
using System.Linq;

namespace BookShopLibrary
{
    public class Delivery
    {
        private static readonly Random random = new Random();

        public Book Book { get; set; }
        public bool IsOrdered { get; set; }
        public DeliveryErrorType ErrorType { get; set; }

        public Delivery(Book book, bool isOrdered, DeliveryErrorType errorType = DeliveryErrorType.None)
        {
            Book = book; IsOrdered = isOrdered; ErrorType = errorType;
        }

        public static Delivery GenerateRandomDelivery(int id)
        {
            var pairs = DatabaseManager.BookAuthorPairs;
            var genres = DatabaseManager.Genres;

            var pair = pairs[random.Next(pairs.Count)];
            string genre = genres[random.Next(genres.Count)];
            int pages = random.Next(100, 800);
            decimal baseCost = random.Next(100, 1000);

            Book book = new Book(id, pair.Title, pair.Author, genre, pages, baseCost);
            
            DeliveryErrorType error = DeliveryErrorType.None;
            int chance = random.Next(100);
            if (chance < 15) error = DeliveryErrorType.Plagiarism;
            else if (chance < 30) error = DeliveryErrorType.Typo;

            InjectError(book, error);
            return new Delivery(book, false, error);
        }

        private static void InjectError(Book book, DeliveryErrorType errorToInject)
        {
            if (errorToInject == DeliveryErrorType.Plagiarism)
            {
                // Плагиат: перемешивание автора
                var otherAuthors = DatabaseManager.BookAuthorPairs
                    .Select(p => p.Author).Where(a => a != book.Author).Distinct().ToList();

                book.Author = otherAuthors.Count > 0 ? otherAuthors[random.Next(otherAuthors.Count)] : "Плагиатор";
            }
            else if (errorToInject == DeliveryErrorType.Typo)
            {
                char[] titleArray = book.Title.ToCharArray();
                int indexToChange = random.Next(0, titleArray.Length);
                char originalChar = titleArray[indexToChange];
                char newChar;
                
                do
                {
                    newChar = (char)random.Next('а', 'я' + 1); 
                } 
                while (newChar == char.ToLower(originalChar) || !char.IsLetter(originalChar)); 
                
                if (char.IsUpper(originalChar)) newChar = char.ToUpper(newChar);
                titleArray[indexToChange] = newChar;
                book.Title = new string(titleArray);
            }
        }
    }
}