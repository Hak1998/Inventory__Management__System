using MyApp.Application.DTOs;

namespace MyApp.Application.Services
{
    public interface ITransactionService
    {
        Task<Response<List<TransactionDto>>> GetAllTransactionsAsync();
        Task<Response<TransactionDto>> GetTransactionByIdAsync(int id);
        Task<Response<int>> CreateTransactionAsync(TransactionDto transactionDto);
    }
}
