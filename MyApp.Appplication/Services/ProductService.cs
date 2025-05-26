using AutoMapper;
using Microsoft.AspNetCore.Http;
using MyApp.Application.DTOs;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using System.Security.Claims;

namespace MyApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly HttpContext _httpContext;

        public ProductService(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnitOfWork uow)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _uow = uow;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<Response<List<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var productDtos = _mapper.Map<List<ProductDto>>(products);

                return Response<List<ProductDto>>.Success(productDtos);
            }
            catch (Exception ex)
            {
                return Response<List<ProductDto>>.Fail(ex.Message, "PRODUCTS_RETRIEVAL_ERROR");
            }
        }

        public async Task<Response<ProductDto>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);

                return product == null
                    ? Response<ProductDto>.Fail("Product not found", "PRODUCT_NOT_FOUND")
                    : Response<ProductDto>.Success(_mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                return Response<ProductDto>.Fail(ex.Message, "PRODUCT_RETRIEVAL_ERROR");
            }
        }

        public async Task<Response<int>> CreateProductAsync(ProductDto productDto)
        {
            try
            {
                var product = new Product
                {
                    Name = productDto.Name,                   
                    Description = productDto.Description,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity,
                    CategoryId = productDto.CategoryId,
                    SupplierId = productDto.SupplierId,
                    CreateUser = GetUserName(),
                };

                await _productRepository.AddAsync(product);
                await _uow.SaveChangesAsync();

                return Response<int>.Success(product.Id, "Product created successfully");
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message, "PRODUCT_CREATION_ERROR");
            }
        }

        public async Task<Response<bool>> UpdateProductAsync(ProductDto productDto)
        {
            try
            {
                var product = await _productRepository.GetProductByIdForUpdAsync(productDto.Id.Value);

                if (product == null)
                    return Response<bool>.Fail("Product not found", "PRODUCT_NOT_FOUND");

                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.Price = productDto.Price;
                product.StockQuantity = productDto.StockQuantity;
                product.ModifyDate = DateTime.UtcNow;
                product.ModifyUser = GetUserName();

                await _uow.SaveChangesAsync();

                return Response<bool>.Success(true, "Product updated successfully");
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.Message, "PRODUCT_UPDATE_ERROR");
            }
        }

        public async Task<Response<bool>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdForUpdAsync(id);

                if (product == null)
                    return Response<bool>.Fail("Product not found", "PRODUCT_NOT_FOUND");

                product.ModifyDate = DateTime.UtcNow;
                product.ModifyUser = GetUserName();
                product.IsActive = false;

                await _uow.SaveChangesAsync();

                return Response<bool>.Success(true, "Product deactivated successfully");
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.Message, "PRODUCT_DEACTIVATION_ERROR");
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
