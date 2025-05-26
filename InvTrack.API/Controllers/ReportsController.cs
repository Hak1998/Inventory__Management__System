using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;

namespace InvTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("sales")]
        public async Task<IActionResult> DownloadSalesReport([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] string format = "pdf")
        {
            var report = await _reportService.GenerateSalesReport(fromDate, toDate);
            return File(report.Content, report.ContentType, report.FileName);
        }

        [HttpGet("purchases")]
        public async Task<IActionResult> DownloadPurchasesReport([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] string format = "pdf")
        {
            var report = await _reportService.GeneratePurchasesReport(fromDate, toDate);
            return File(report.Content, report.ContentType, report.FileName);
        }

        [HttpGet("stock")]
        public async Task<IActionResult> DownloadStockLevelsReport()
        {
            var report = await _reportService.GenerateStockLevelsReport();
            return File(report.Content, report.ContentType, report.FileName);
        }
    }
}
