
using MyApp.Application.DTOs;
using MyApp.Application.Enums;
using MyApp.Application.Interfaces;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;
using System.Text;

namespace MyApp.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPdfGenerator _pdfGenerator;

        public ReportService(ITransactionRepository transactionRepository,
                           IProductRepository productRepository,
                           IPdfGenerator pdfGenerator)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _pdfGenerator = pdfGenerator;
        }

        public async Task<ReportResult> GenerateSalesReport(DateTime fromDate, DateTime toDate)
        {
            var transactions = await _transactionRepository.GetByDateRangeAsync(fromDate, toDate, TransactionType.Sale.ToString());
            var result = GenerateTransactionReport(transactions, "Sales");

            return result;
        }

        public async Task<ReportResult> GeneratePurchasesReport(DateTime fromDate, DateTime toDate)
        {
            var transactions = await _transactionRepository.GetByDateRangeAsync(fromDate, toDate, TransactionType.Purchase.ToString());
            var result = GenerateTransactionReport(transactions, "Purchases");

            return result;
        }

        public async Task<ReportResult> GenerateStockLevelsReport()
        {
            var products = await _productRepository.GetAllAsync();

            // CSV Generation
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Product ID,Name,Category,Supplier,Price,Stock Quantity");

            foreach (var product in products)
            {
                csvBuilder.AppendLine($"{product.Id},{product.Name},{product.Category?.Name}," +
                                    $"{product.Supplier?.Name},{product.Price}," +
                                    $"{product.StockQuantity}");
            }

            return new ReportResult
            {
                Content = Encoding.UTF8.GetBytes(csvBuilder.ToString()),
                ContentType = "text/csv",
                FileName = $"StockLevels_{DateTime.Now:yyyyMMdd}.csv"
            };
        }

        private ReportResult GenerateTransactionReport(IEnumerable<Transaction> transactions, string reportType)
        {
            // For PDF generation
            var pdfContent = _pdfGenerator.GenerateTransactionPdf(transactions, reportType);

            return new ReportResult
            {
                Content = pdfContent,
                ContentType = "application/pdf",
                FileName = $"{reportType}_Report_{DateTime.Now:yyyyMMdd}.pdf"
            };
        }
    }
}
