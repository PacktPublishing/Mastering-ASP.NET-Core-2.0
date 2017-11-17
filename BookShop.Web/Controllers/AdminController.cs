using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookShop.Web.Services;
using BookShop.DomainModel;

namespace BookShop.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IBookService _svc;

        public AdminController(IBookService svc)
        {
            this._svc = svc;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Book()
        {
            var authors = this._svc.GetAuthors();
            ViewBag.Authors = authors;
            return this.View();
        }

        public IActionResult Author()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Book(Book book, int author)
        {
            book.Author = new Author { AuthorId = author };

            this._svc.AddBook(book);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Author(Author author)
        {
            this._svc.AddAuthor(author);

            return this.RedirectToAction("Index");
        }
    }
}