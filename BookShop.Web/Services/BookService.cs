using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookShop.Web.Services
{
    public class BookService : IBookService
    {
        private readonly BookShopContext _ctx;
        private readonly DiagnosticSource _diagnosticSource;

        public BookService(BookShopContext ctx, DiagnosticSource diagnosticSource)
        {
            this._ctx = ctx;
            this._diagnosticSource = diagnosticSource;
        }

        public void AddAuthor(Author author)
        {
            this._ctx.Authors.Add(author);
            this._ctx.SaveChanges();
            this._diagnosticSource.Write("OnAuthorCreated", new { authorId = author.AuthorId });
        }

        public void AddBook(Book book)
        {
            book.Author = this._ctx.Authors.Find(book.Author.AuthorId);
            this._ctx.Books.Add(book);
            this._ctx.SaveChanges();
            this._diagnosticSource.Write("OnBookCreated", new { bookId = book.BookId });

        }

        private User GetUser(string email)
        {
            var user = this._ctx.Users.Where(x => x.Email == email).SingleOrDefault();
            
            return user;
        }

        public int CreateOrder(ShoppingBag bag, string email)
        {
            var user = this.GetUser(email);
            var order = new Order { State = State.Received, User = user, Timestamp = DateTime.UtcNow };
            var books = this.GetBooksByIds(bag.BookIds);

            foreach (var item in bag.BookIds)
            {
                var orderItem = new OrderItem { Order = order, Book = books.Single(x => x.BookId == item), Quantity = bag.Quantity(item) };
                order.Items.Add(orderItem);
            }

            user.Orders.Add(order);

            this._ctx.SaveChanges();

            this._diagnosticSource.Write("OnOrderCreated", new { orderId = order.OrderId });

            return order.OrderId;
        }

        public IEnumerable<Author> GetAuthors()
        {
            return this._ctx.Authors.OrderBy(x => x.Name).ToList();
        }

        public Book GetBook(int bookId)
        {
            return this._ctx.Books.Where(x => x.BookId == bookId).Include(x => x.Author).Single();
        }

        public IEnumerable<Book> GetTopBooks()
        {
            var books = this._ctx.Books.Include(x => x.Author).OrderByDescending(x => x.ReleaseDate).Take(5).ToList();
            return books;
        }

        public IEnumerable<Book> Search(string search)
        {
            var books = this
                ._ctx
                .Books
                .Include(x => x.Author)
                .Where(x => x.Title.Contains(search) || x.Description.Contains(search) || x.Author.Name.Contains(search))
                .OrderByDescending(x => x.ReleaseDate)
                .Take(5)
                .ToList();
            return books;
        }

        public IEnumerable<Book> GetBooksByAuthor(int id)
        {
            var books = this._ctx.Books.Where(x => x.Author.AuthorId == id).Include(x => x.Author).OrderBy(x => x.ReleaseDate).ToList();
            return books;
        }

        public IEnumerable<Book> GetBooksByIds(IEnumerable<int> ids)
        {
            var books = this._ctx.Books.Where(x => ids.Contains(x.BookId)).Include(x => x.Author).OrderBy(x => x.ReleaseDate).ToList();

            return books;
        }

        public void AddBookReview(string email, int bookId, string content, int stars)
        {
            var book = this.GetBook(bookId);
            var user = this._ctx.Users.Where(x => x.Email == email).SingleOrDefault();
            var review = new Review { User = user, Content = content, Stars = stars };
            user.Reviews.Add(review);
            this._ctx.SaveChanges();
        }
    }
}
