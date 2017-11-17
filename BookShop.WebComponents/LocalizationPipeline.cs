using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace BookShop.WebComponents
{
    public sealed class LocalizationPipeline
    {
        public static void Configure(IApplicationBuilder app, IOptions<RequestLocalizationOptions> options)
        {
            app.UseRequestLocalization(options.Value);
        }
    }
}