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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CategoryDto> _validator;

        public CategoriesController(ICategoryService categoryService, IValidator<CategoryDto> validator)
        {
            _categoryService = categoryService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var response = await _categoryService.GetAllCategoriesAsync();

            return response.Status.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);

            if (!response.Status.Success)
                return response.Status.ErrorCode == "CATEGORY_NOT_FOUND"
                    ? NotFound(response)
                    : BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryModel categoryModel)
        {
            var categoryDto = new CategoryDto
            {
                Name = categoryModel.Name,
                Description = categoryModel.Description
            };

            var validationResult = await _validator.ValidateAsync(categoryDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _categoryService.CreateCategoryAsync(categoryDto);

            return response.Status.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryModel categoryModel)
        {
            var categoryDto = new CategoryDto
            {
                Id = id,
                Name = categoryModel.Name,
                Description = categoryModel.Description
            };

            var validationResult = await _validator.ValidateAsync(categoryDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _categoryService.UpdateCategoryAsync(categoryDto);

            if (!response.Status.Success)
                return response.Status.ErrorCode == "CATEGORY_NOT_FOUND"
                    ? NotFound(response)
                    : BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);

            if (!response.Status.Success)
                return response.Status.ErrorCode == "CATEGORY_NOT_FOUND"
                    ? NotFound(response)
                    : BadRequest(response);

            return Ok(response);
        }
    }
}
