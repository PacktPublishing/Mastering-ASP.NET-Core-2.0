using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookShop.Web.Models;
using System.Dynamic;
using BookShop.Web.Services;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using BookShop.WebComponents;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace BookShop.Web.Controllers
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class HomeController : Controller
    {
        private readonly IBookService _svc;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IBookService svc, IStringLocalizer<HomeController> localizer)
        {
            this._svc = svc;
            this._localizer = localizer;
        }

        public IActionResult Empty()
        {
            var cart = Web.ShoppingBag.Get(this.HttpContext);
            cart.Clear();
            cart.Save(this.HttpContext);

            return this.RedirectToAction("Index");
        }

        public IActionResult ShoppingBag()
        {
            var cart = Web.ShoppingBag.Get(this.HttpContext);
            var items = this._svc.GetBooksByIds(cart.BookIds);

            dynamic model = new ExpandoObject();
            model.Cart = cart;
            model.Items = items;
        
            return this.View("ShoppingBag", model);
        }

        public IActionResult Remove(int id)
        {
            var cart = Web.ShoppingBag.Get(this.HttpContext);
            cart.Remove(id);
            cart.Save(this.HttpContext);

            this.ViewBag.Message = "Item removed from the bag";

            if (cart.IsEmpty == true)
            {
                return this.RedirectToAction("Index");
            }
            else
            {
                return this.ShoppingBag();
            }
        }

        public IActionResult Buy(int id, string returnUrl)
        {
            var cart = Web.ShoppingBag.Get(this.HttpContext);
            cart.Add(id);
            cart.Save(this.HttpContext);

            return this.Redirect(returnUrl);
        }

        public IActionResult Search(string search)
        {
            var books = this._svc.Search(search);
            dynamic model = new ExpandoObject();
            model.Books = books;
            model.Search = search;
            return this.View("Results", model);
        }

        public IActionResult Index()
        {
            var books = this._svc.GetTopBooks();

            return this.View(books);
        }

        private string GetUserEmail()
        {
            var claims = this.User.Claims.ToArray();
            var claim = this.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email);
            return claim?.Value;
        }

        private string GetUserName()
        {
            var claims = this.User.Claims.ToArray();
            var claim = this.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name);
            return claim?.Value;
        }

        [Authorize]
        public IActionResult Order()
        {
            var cart = Web.ShoppingBag.Get(this.HttpContext);

            this._svc.CreateOrder(cart, this.GetUserEmail());

            cart.Clear();
            cart.Save(this.HttpContext);

            this.ViewBag.Message = "Your order was placed";

            return this.View("ShoppingBag");
        }

        [ResponseCache(CacheProfileName = "Public1Hour")]
        public IActionResult Detail(int id)
        {
            var book = this._svc.GetBook(id);

            return this.View(book);
        }

        public IActionResult Author(int id)
        {
            var books = this._svc.GetBooksByAuthor(id);

            return this.View(books);
        }

        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Review(int bookId, string content, int stars)
        {
            this._svc.AddBookReview(this.GetUserEmail(), bookId, content, stars);

            return this.RedirectToAction("Index");
        }

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult CatchAll()
        {
            return this.View();
        }
    }
}
