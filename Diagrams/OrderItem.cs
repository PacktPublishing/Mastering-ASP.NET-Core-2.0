namespace BookShop.DomainModel
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public Order Order { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}