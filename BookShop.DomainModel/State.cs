namespace BookShop.DomainModel
{
    public enum State
    {
        None = 0,
        Received = 1,
        Paid = 2,
        Sent = 3,
        Returned = 4,
        Cancelled = 5
    }
}