using Microsoft.EntityFrameworkCore;
namespace Option1.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
        public DbSet<Products> products { get; set; }
        public DbSet<Orders> orders { get; set; }
    }
}
