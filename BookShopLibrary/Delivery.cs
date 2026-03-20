using System;

namespace BookShopLibrary
{
    public class Delivery
    {
        public Book Book { get; set; }
        public bool IsOrdered { get; set; }

        public Delivery(Book book, bool isOrdered)
        {
            Book = book ?? throw new ArgumentNullException(nameof(book));
            IsOrdered = isOrdered;
        }
    }
}