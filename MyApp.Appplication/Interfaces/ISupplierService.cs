using MyApp.Application.DTOs;

namespace MyApp.Application.Services
{
    public interface ISupplierService
    {
        Task<Response<List<SupplierDto>>> GetAllSuppliersAsync();
        Task<Response<SupplierDto>> GetSupplierByIdAsync(int id);
        Task<Response<int>> CreateSupplierAsync(SupplierDto supplier);
        Task<Response<bool>> UpdateSupplierAsync(SupplierDto supplier);
        Task<Response<bool>> DeleteSupplierAsync(int id);
    }
}
