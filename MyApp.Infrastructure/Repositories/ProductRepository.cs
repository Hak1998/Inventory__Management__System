using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Extensions;

namespace MyApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext _context;

        public ProductRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Where(p => p.IsActive).Include(p => p.Category).Include(p => p.Supplier).AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Where(p => p.Id == id && p.IsActive).Include(p => p.Category).Include(p => p.Supplier).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Product product)
        {

            await _context.Products.AddAsync(product);
        }

        public async Task<Product> GetProductByIdForUpdAsync(int id)
        {
            return await _context.Products.WithUpdLockNoWait($"\"Id\" = {id}").FirstOrDefaultAsync();
        }
    }
}
