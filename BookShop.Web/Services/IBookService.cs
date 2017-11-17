using BookShop.DomainModel;
using System.Collections.Generic;

namespace BookShop.Web.Services
{
    public interface IBookService
    {
        int CreateOrder(ShoppingBag bag, string email);
        IEnumerable<Book> GetTopBooks();
        IEnumerable<Book> Search(string search);
        Book GetBook(int id);
        IEnumerable<Book> GetBooksByAuthor(int id);
        IEnumerable<Book> GetBooksByIds(IEnumerable<int> ids);
        IEnumerable<Author> GetAuthors();
        void AddBook(Book book);
        void AddAuthor(Author author);
        void AddBookReview(string email, int bookId, string content, int stars);
    }
}
