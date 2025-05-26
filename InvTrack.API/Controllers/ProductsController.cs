using FluentValidation;
using InvTrack.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Services;

namespace InvTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidator<ProductDto> _validator;

        public ProductsController(IProductService productService, IValidator<ProductDto> validator)
        {
            _productService = productService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var response = await _productService.GetAllProductsAsync();

            return response.Status.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);

            if (!response.Status.Success)
                return response.Status.ErrorCode == "PRODUCT_NOT_FOUND"
                    ? NotFound(response)
                    : BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductModel productModel)
        {
            var productDto = new ProductDto
            {
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                StockQuantity = productModel.StockQuantity,
                CategoryId = productModel.CategoryId,
                SupplierId = productModel.SupplierId
            };

            var validationResult = await _validator.ValidateAsync(productDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _productService.CreateProductAsync(productDto);

            return response.Status.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductModel productModel)
        {
            var productDto = new ProductDto
            {
                Id = id,
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                StockQuantity = productModel.StockQuantity,
                CategoryId = productModel.CategoryId,
                SupplierId = productModel.SupplierId
            };

            var validationResult = await _validator.ValidateAsync(productDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _productService.UpdateProductAsync(productDto);

            if (!response.Status.Success)
                return response.Status.ErrorCode == "PRODUCT_NOT_FOUND"
                    ? NotFound(response)
                    : BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.DeleteProductAsync(id);

            if (!response.Status.Success)
                return response.Status.ErrorCode == "PRODUCT_NOT_FOUND"
                    ? NotFound(response)
                    : BadRequest(response);

            return Ok(response);
        }
    }
}
