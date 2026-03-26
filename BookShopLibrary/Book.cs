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

        public override string ToString() => $"{Id}: {DisplayTitle} - {Author} ({Genre}) | Себестоимость: {BaseCost:C}";
    }
}