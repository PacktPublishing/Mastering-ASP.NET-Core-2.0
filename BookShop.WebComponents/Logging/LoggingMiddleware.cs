using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BookShop.WebComponents.Logging
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            this._loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var logger = this._loggerFactory.CreateLogger<LoggingMiddleware>();

            using (logger.BeginScope<LoggingMiddleware>(this))
            {
                logger.LogInformation("Before");
                await this._next.Invoke(context);
                logger.LogInformation("After");
            }
        }
    }
}