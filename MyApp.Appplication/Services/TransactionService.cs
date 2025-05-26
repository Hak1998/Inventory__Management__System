using AutoMapper;
using MyApp.Application.DTOs;
using MyApp.Application.Enums;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;

namespace MyApp.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public TransactionService(ITransactionRepository transactionRepository, IProductRepository productRepository, IMapper mapper, IUnitOfWork uow)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<Response<List<TransactionDto>>> GetAllTransactionsAsync()
        {
            try
            {
                var transactions = await _transactionRepository.GetAllAsync();
                var transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);

                return Response<List<TransactionDto>>.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                return Response<List<TransactionDto>>.Fail("Failed to retrieve transactions", "TRANSACTIONS_RETRIEVAL_ERROR");
            }
        }

        public async Task<Response<TransactionDto>> GetTransactionByIdAsync(int id)
        {
            try
            {
                var transaction = await _transactionRepository.GetByIdAsync(id);

                return transaction == null
                    ? Response<TransactionDto>.Fail("Transaction not found", "TRANSACTION_NOT_FOUND")
                    : Response<TransactionDto>.Success(_mapper.Map<TransactionDto>(transaction));
            }
            catch (Exception ex)
            {
                return Response<TransactionDto>.Fail("Failed to retrieve transaction", "TRANSACTION_RETRIEVAL_ERROR");
            }
        }

        public async Task<Response<int>> CreateTransactionAsync(TransactionDto transactionDto)
        {
            try
            {
                var product = await _productRepository.GetProductByIdForUpdAsync(transactionDto.ProductId);

                if (product == null)
                {
                    return Response<int>.Fail("Product not found", "PRODUCT_NOT_FOUND");
                }

                if (transactionDto.TransactionType.Equals(TransactionType.Purchase.ToString(), StringComparison.OrdinalIgnoreCase))
                    product.StockQuantity += transactionDto.Quantity;
                else if (transactionDto.TransactionType.Equals(TransactionType.Sale.ToString(),StringComparison.OrdinalIgnoreCase))
                {
                    if (product.StockQuantity < transactionDto.Quantity)
                    {
                        return Response<int>.Fail("Product is out of stock", "OUT_OF_STOCK");
                    }

                    product.StockQuantity -= transactionDto.Quantity;
                }

                var transaction = new Transaction
                {
                    ProductId = transactionDto.ProductId,
                    Quantity = transactionDto.Quantity,
                    TransactionType = transactionDto.TransactionType,
                    Notes = transactionDto.Notes
                };

                await _transactionRepository.AddAsync(transaction);
                await _uow.SaveChangesAsync();

                return Response<int>.Success(transaction.Id);
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message, "SYSTEM_ERROR");
            }
        }
    }
}
