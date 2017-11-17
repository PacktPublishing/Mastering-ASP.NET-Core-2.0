using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookShop.WebComponents.ValueProviders
{
    public class CookieValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            context.ValueProviders.Add(new CookieValueProvider(context.ActionContext.HttpContext));
            return Task.CompletedTask;
        }
    }
}