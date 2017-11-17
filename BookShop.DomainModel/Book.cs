using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.DomainModel
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public int Stock { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Author Author { get; set; }
        public ICollection<Review> Reviews { get; private set; } = new HashSet<Review>();
        public ICollection<OrderItem> Items { get; private set; } = new HashSet<OrderItem>();
        public override string ToString() => Title;
    }
}
