using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.DomainModel
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public ICollection<Book> Books { get; private set; } = new HashSet<Book>();

        public override string ToString() => Name;
    }
}
