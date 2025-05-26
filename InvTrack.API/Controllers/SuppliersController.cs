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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IValidator<SupplierDto> _validator;

        public SuppliersController(ISupplierService supplierService, IValidator<SupplierDto> validator)
        {
            _supplierService = supplierService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var response = await _supplierService.GetAllSuppliersAsync();

            return response.Status.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _supplierService.GetSupplierByIdAsync(id);

            if (!response.Status.Success)
                return response.Status.ErrorCode == "SUPPLIER_NOT_FOUND"
                    ? NotFound(response)
                    : BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(SupplierModel supplierModel)
        {
            var supplierDto = new SupplierDto
            {
                Name = supplierModel.Name,
                Email = supplierModel.Email,
                Phone = supplierModel.Phone,
                Address = supplierModel.Address
            };

            var validationResult = await _validator.ValidateAsync(supplierDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _supplierService.CreateSupplierAsync(supplierDto);

            return response.Status.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SupplierModel supplierModel)
        {
            var supplierDto = new SupplierDto
            {
                Id = id,
                Name = supplierModel.Name,
                Email = supplierModel.Email,
                Phone = supplierModel.Phone,
                Address = supplierModel.Address
            };

            var validationResult = await _validator.ValidateAsync(supplierDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _supplierService.UpdateSupplierAsync(supplierDto);

            if (!response.Status.Success)
                return response.Status.ErrorCode == "SUPPLIER_NOT_FOUND"
                    ? NotFound(response)
                    : BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _supplierService.DeleteSupplierAsync(id);

            if (!response.Status.Success)
                return response.Status.ErrorCode == "SUPPLIER_NOT_FOUND"
                    ? NotFound(response)
                    : BadRequest(response);

            return Ok(response);
        }
    }
}
