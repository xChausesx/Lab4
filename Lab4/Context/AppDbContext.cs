using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Keyword> Keywords{ get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }
    }
}
