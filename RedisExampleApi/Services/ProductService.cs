using RedisExampleApi.Models;
using RedisExampleApi.Repository;

namespace RedisExample.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<Product> CreateAsync(Product product)
        {
            return _productRepository.CreateAsync(product);
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _productRepository.GetAllAsync();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return _productRepository.GetByIdAsync(id);
        }
    }
}
