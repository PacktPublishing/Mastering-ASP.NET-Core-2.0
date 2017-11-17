using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;

namespace BookShop.WebComponents.Theming
{
    public class ThemeViewLocationExpander : IViewLocationExpander
    {
        private readonly string _theme;

        public ThemeViewLocationExpander(string theme)
        {
            this._theme = theme;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var theme = context.Values["theme"];
            return viewLocations
                .Select(x => x.Replace("/Views/", "/Views/" + theme + "/"))
                .Concat(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["theme"] = this._theme;
        }
    }
}