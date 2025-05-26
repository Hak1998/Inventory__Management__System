using AutoMapper;
using Microsoft.AspNetCore.Http;
using MyApp.Application.DTOs;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using System.Security.Claims;

namespace MyApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly HttpContext _httpContext;

        public CategoryService(ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnitOfWork uow)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _uow = uow;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<Response<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

                return Response<List<CategoryDto>>.Success(categoryDtos);
            }
            catch (Exception ex)
            {
                return Response<List<CategoryDto>>.Fail(ex.Message, "CATEGORIES_RETRIEVAL_ERROR");
            }
        }

        public async Task<Response<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);

                return category == null
                    ? Response<CategoryDto>.Fail("Category not found", "CATEGORY_NOT_FOUND")
                    : Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category));
            }
            catch (Exception ex)
            {
                return Response<CategoryDto>.Fail(ex.Message, "CATEGORY_RETRIEVAL_ERROR");
            }
        }

        public async Task<Response<int>> CreateCategoryAsync(CategoryDto categoryDto)
        {
            try
            {
                var category = new Category
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                    CreateUser = GetUserName()
                };

                await _categoryRepository.AddAsync(category);
                await _uow.SaveChangesAsync();

                return Response<int>.Success(category.Id, "Category created successfully");
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message, "CATEGORY_CREATION_ERROR");
            }
        }

        public async Task<Response<bool>> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryDto.Id.Value);

                if (category == null)
                    return Response<bool>.Fail("Category not found", "CATEGORY_NOT_FOUND");

                category.Name = categoryDto.Name;
                category.Description = categoryDto.Description;
                category.ModifyDate = DateTime.UtcNow;
                category.ModifyUser = GetUserName();

                await _uow.SaveChangesAsync();

                return Response<bool>.Success(true, "category updated successfully");
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.Message, "CATEGORY_UPDATE_ERROR");
            }
        }

        public async Task<Response<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);

                if (category == null)
                    return Response<bool>.Fail("Category not found", "CATEGORY_NOT_FOUND");

                category.ModifyDate = DateTime.UtcNow;
                category.ModifyUser = GetUserName();
                category.IsActive = false;

                await _uow.SaveChangesAsync();

                return Response<bool>.Success(true, "Category deactivated successfully");
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.Message, "CATEGORY_DEACTIVATION_ERROR");
            }
        }

        private string GetUserName()
        {
            string firstName = _httpContext.User.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;
            string lastName = _httpContext.User.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;

            return $"{firstName} {lastName}".Trim();
        }
    }
}
