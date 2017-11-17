using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace BookShop.WebComponents.TagHelpers
{
    public class UserTagHelper : TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = this.ViewContext.HttpContext.User.Identity.Name;

            output.Content.Append(user);

            return base.ProcessAsync(context, output);
        }
    }
}
