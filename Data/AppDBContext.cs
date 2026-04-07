using Microsoft.EntityFrameworkCore;

namespace EFCoreDeepDive.Data

{
    public class AppDBContext : DbContext
    {
        // This is an explict constructor, the options will be provided by the
        // DI container when an instance of this class is created.
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(
                new Currency() { Id = 1, Title = "INR", Description = "Indian INR" },
                new Currency() { Id = 2, Title = "Dollar", Description = "Dollar" },
                new Currency() { Id = 3, Title = "Euro", Description = "Euro" },
                new Currency() { Id = 4, Title = "Dinar", Description = "Dinar" }
                );

            modelBuilder.Entity<Language>().HasData(
               new Language() { Id = 1, Title = "Hindi", Description = "Hindi" },
               new Language() { Id = 2, Title = "Tamil", Description = "Tamil" },
               new Language() { Id = 3, Title = "Punjabi", Description = "Punjabi" },
               new Language() { Id = 4, Title = "Urdu", Description = "Urdu" }
               );
        }

        // DBSet is given the type book
        // and the table nae is set as Books
        public DbSet<Book> Books { get; set; }

        public DbSet<Language> Languages { get; set; }
        public DbSet<BookPrice> BookPrices { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
