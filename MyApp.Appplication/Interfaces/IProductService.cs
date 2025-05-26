using MyApp.Application.DTOs;

namespace MyApp.Application.Services
{
    public interface IProductService
    {
        Task<Response<List<ProductDto>>> GetAllProductsAsync();
        Task<Response<ProductDto>> GetProductByIdAsync(int id);
        Task<Response<int>> CreateProductAsync(ProductDto product);
        Task<Response<bool>> UpdateProductAsync(ProductDto product);
        Task<Response<bool>> DeleteProductAsync(int id);
    }
}
