using System.Collections.Generic;

namespace BookShop.DomainModel
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<Review> Reviews { get; private set; } = new HashSet<Review>();
        public ICollection<Order> Orders { get; private set; } = new HashSet<Order>();

        public override string ToString() => Name;
    }
}