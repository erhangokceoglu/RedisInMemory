using Microsoft.EntityFrameworkCore;
using RedisExampleApi.Models;

namespace RedisExampleApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return (await _dbContext.Products.FindAsync(id))!;
        }
    }
}
