using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace BookShop.WebComponents.ValueProviders
{
    public class CookieValueProvider : IValueProvider
    {
        private readonly HttpContext _context;

        public CookieValueProvider(HttpContext context)
        {
            this._context = context;
        }

        public bool ContainsPrefix(string prefix)
        {
            return this._context.Request.Cookies.ContainsKey(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            return new ValueProviderResult(this._context.Request.Cookies[key]);
        }
    }
}