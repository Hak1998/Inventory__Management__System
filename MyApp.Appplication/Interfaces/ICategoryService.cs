using MyApp.Application.DTOs;

namespace MyApp.Application.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllCategoriesAsync();
        Task<Response<CategoryDto>> GetCategoryByIdAsync(int id);
        Task<Response<int>> CreateCategoryAsync(CategoryDto category);
        Task<Response<bool>> UpdateCategoryAsync(CategoryDto category);
        Task<Response<bool>> DeleteCategoryAsync(int id);
    }
}
