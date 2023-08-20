using Microsoft.EntityFrameworkCore;

namespace RedisExampleApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seed
            modelBuilder.Entity<Product>().HasData
            (
            new Product() { Id = 1, Name = "Kalem", Price = 50 },
            new Product() { Id = 2, Name = "Silgi", Price = 40 },
            new Product() { Id = 3, Name = "Defter", Price = 80 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
