using Microsoft.EntityFrameworkCore;

namespace BookShop.DomainModel
{
    public class BookShopContext : DbContext
    {
        public BookShopContext(DbContextOptions<BookShopContext> options) : base(options)
        {
        }

        private static DbContextOptions<BookShopContext> GetOptions(string connectionString)
        {
            var options = new DbContextOptionsBuilder<BookShopContext>()
                .UseSqlServer(connectionString, x => x.MigrationsAssembly("BookShop.Web"))
                .Options;

            return options;
        }

        public DbSet<User> Users { get; private set; }
        public DbSet<Book> Books { get; private set; }
        public DbSet<Order> Orders { get; private set; }
        public DbSet<Review> Reviews { get; private set; }
        public DbSet<Author> Authors { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Author>()
                .HasMany(x => x.Books)
                .WithOne(x => x.Author);

            base.OnModelCreating(modelBuilder);
        }
    }
}
