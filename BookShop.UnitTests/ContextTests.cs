using BookShop.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using Xunit;

namespace BookShop.UnitTests
{
    public class ContextTests
    {
        private static BookShopContext GetContext()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var loggerFactory = new LoggerFactory()
                .AddConsole((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name))
                .AddDebug((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name));

            var connectionString = configuration["ConnectionStrings:BookShop"];

            var options = new DbContextOptionsBuilder<BookShopContext>()
                .UseSqlServer(connectionString)
                .UseLoggerFactory(loggerFactory)
                .Options;

            return new BookShopContext(options);
        }

        [Fact]
        public void CanGetBooks()
        {
            using (var ctx = GetContext())
            {
                var books = ctx.Books.ToList();

                Assert.NotEmpty(books);
            }
        }

        [Fact]
        public void CanGetBooksByAuthor()
        {
            using (var ctx = GetContext())
            {
                var books = ctx.Books.Where(x => x.Author.AuthorId == 1).ToList();

                Assert.NotEmpty(books);
            }
        }
    }
}
