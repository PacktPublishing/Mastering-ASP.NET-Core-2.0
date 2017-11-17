using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BookShop.Web.TagHelpers
{
    [HtmlTargetElement("shopping-bag-items")]
    public class ShoppingBagItemsTagHelper : TagHelper
    {
        public ShoppingBagItemsTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContext = httpContextAccessor.HttpContext;
        }

        public HttpContext HttpContext { get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var cart = ShoppingBag.Get(this.HttpContext);

            output.TagName = "a";
            output.Attributes.Add("href", "/Home/ShoppingBag");
            output.Content.SetContent($"Items: {cart.Count}");

            base.Process(context, output);
        }
    }
}
