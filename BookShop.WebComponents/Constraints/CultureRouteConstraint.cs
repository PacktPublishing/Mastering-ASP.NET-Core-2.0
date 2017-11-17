using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;

namespace BookShop.WebComponents.Constraints
{
    public sealed class CultureRouteConstraint : IRouteConstraint
    {
        public const string CultureKey = "culture";

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if ((values.ContainsKey(CultureKey) == false) || (values[CultureKey] == null))
            {
                return false;
            }

            var lang = values[CultureKey].ToString();

            var requestLocalizationOptions = httpContext.RequestServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();

            if ((requestLocalizationOptions.Value.SupportedCultures == null) || (requestLocalizationOptions.Value.SupportedCultures.Count == 0))
            {
                try
                {
                    new System.Globalization.CultureInfo(lang);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return requestLocalizationOptions.Value.SupportedCultures.Any(culture => culture.Name.Equals(lang, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}