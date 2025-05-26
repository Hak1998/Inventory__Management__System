using AutoMapper;
using Microsoft.AspNetCore.Http;
using MyApp.Application.DTOs;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using System.Security.Claims;

namespace MyApp.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly HttpContext _httpContext;

        public SupplierService(ISupplierRepository supplierRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnitOfWork uow)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _uow = uow;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<Response<List<SupplierDto>>> GetAllSuppliersAsync()
        {
            try
            {
                var suppliers = await _supplierRepository.GetAllAsync();
                var supplierDtos = _mapper.Map<List<SupplierDto>>(suppliers);

                return Response<List<SupplierDto>>.Success(supplierDtos);
            }
            catch (Exception ex)
            {
                return Response<List<SupplierDto>>.Fail(ex.Message, "SUPPLIERS_RETRIEVAL_ERROR");
            }
        }

        public async Task<Response<SupplierDto>> GetSupplierByIdAsync(int id)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(id);

                return supplier == null
                    ? Response<SupplierDto>.Fail("Supplier not found", "SUPPLIER_NOT_FOUND")
                    : Response<SupplierDto>.Success(_mapper.Map<SupplierDto>(supplier));
            }
            catch (Exception ex)
            {
                return Response<SupplierDto>.Fail(ex.Message, "SUPPLIER_RETRIEVAL_ERROR");
            }
        }

        public async Task<Response<int>> CreateSupplierAsync(SupplierDto supplierDto)
        {
            try
            {
                var supplier = new Supplier
                {
                    Name = supplierDto.Name,
                    Email = supplierDto.Email,
                    Phone = supplierDto.Phone,
                    Address = supplierDto.Address,
                    CreateUser = GetUserName()
                };

                await _supplierRepository.AddAsync(supplier);
                await _uow.SaveChangesAsync();

                return Response<int>.Success(supplier.Id, "Supplier created successfully");
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message, "SUPPLIER_CREATION_ERROR");
            }
        }

        public async Task<Response<bool>> UpdateSupplierAsync(SupplierDto supplierDto)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(supplierDto.Id.Value);

                if (supplier == null)
                    return Response<bool>.Fail("Supplier not found", "SUPPLIER_NOT_FOUND");

                supplier.Name = supplierDto.Name;
                supplier.Email = supplierDto.Email;
                supplier.Phone = supplierDto.Phone;
                supplier.Address = supplierDto.Address;
                supplier.ModifyDate = DateTime.UtcNow;
                supplier.ModifyUser = GetUserName();

                await _uow.SaveChangesAsync();

                return Response<bool>.Success(true, "Supplier updated successfully");
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.Message, "SUPPLIER_UPDATE_ERROR");
            }
        }

        public async Task<Response<bool>> DeleteSupplierAsync(int id)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(id);

                if (supplier == null)
                    return Response<bool>.Fail("Supplier not found", "SUPPLIER_NOT_FOUND");

                supplier.ModifyDate = DateTime.UtcNow;
                supplier.ModifyUser = GetUserName();
                supplier.IsActive = false;

                await _uow.SaveChangesAsync();

                return Response<bool>.Success(true, "Supplier deactivated successfully");
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.Message, "SUPPLIER_DEACTIVATION_ERROR");
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
