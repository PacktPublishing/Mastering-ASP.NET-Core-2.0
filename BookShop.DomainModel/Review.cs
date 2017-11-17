using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.DomainModel
{
    public class Review
    {
        public int ReviewId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
        public int Stars { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }

        public override string ToString() => Content;
    }
}
