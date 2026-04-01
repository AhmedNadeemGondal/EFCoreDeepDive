using Microsoft.EntityFrameworkCore;

namespace EFCoreDeepDive.Data
   
{
    public class AppDBContext : DbContext
    {
        // This is an explict constructor, the options will be provided by the
        // DI container when an instance of this class is created.
        public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {
        }

        // DBSet is given the type book
        // and the table nae is set as Books
        public DbSet<Book> Books { get; set; }

        public DbSet<Language> Languages { get; set; }
    }
}
