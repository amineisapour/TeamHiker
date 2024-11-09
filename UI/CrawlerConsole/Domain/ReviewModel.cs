using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.Domain
{
    public class Review
    {
        public string Type { get; set; }
        public ItemReviewed ItemReviewed { get; set; }
        public ReviewRating ReviewRating { get; set; }
        public string ReviewBody { get; set; }
        public Author Author { get; set; }
    }

    public class ItemReviewed
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Type { get; set; }
        public string AddressLocality { get; set; }
    }

    public class ReviewRating
    {
        public string Type { get; set; }
        public decimal RatingValue { get; set; }
    }

    public class Author
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class BreadcrumbList
    {
        public string Context { get; set; }
        public string Type { get; set; }
        public List<ItemListElement> ItemListElement { get; set; }
    }

    public class ItemListElement
    {
        public string Type { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Item { get; set; }
    }
}
