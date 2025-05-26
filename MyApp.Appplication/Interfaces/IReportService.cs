using MyApp.Application.DTOs;

namespace MyApp.Application.Interfaces
{
    public interface IReportService
    {
        Task<ReportResult> GenerateSalesReport(DateTime fromDate, DateTime toDate);
        Task<ReportResult> GeneratePurchasesReport(DateTime fromDate, DateTime toDate);
        Task<ReportResult> GenerateStockLevelsReport();
    }
}
