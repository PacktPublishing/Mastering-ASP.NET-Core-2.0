using System;
using System.Collections.Generic;

namespace BookShop.DomainModel
{
    public class Order
    {
        public int OrderId { get; set; }
        public User User { get; set; }
        public DateTime Timestamp { get; set; }
        public State State { get; set; }
        public ICollection<OrderItem> Items { get; private set; } = new HashSet<OrderItem>();
    }
}