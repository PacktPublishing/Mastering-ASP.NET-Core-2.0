using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace BookShop.WebComponents.TagHelpers
{
    [OutputElementHint("span")]
    [HtmlTargetElement("time")]
    public class TimeTagHelper : TagHelper
    {
        public string Format { get; set; } = "HH:mm";

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var time = DateTime.Now.ToString(this.Format);
            output.TagName = "span";
            output.Content.SetContent(time);

            return base.ProcessAsync(context, output);
        }
    }
}
