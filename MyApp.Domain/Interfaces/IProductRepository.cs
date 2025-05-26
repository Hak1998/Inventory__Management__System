using MyApp.Domain.Entities;

namespace MyApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetProductByIdForUpdAsync(int id);
        Task AddAsync(Product product);
    }
}
