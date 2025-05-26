
namespace MyApp.Application.DTOs
{
    public class TransactionDto
    {
        public int? Id { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
        public string TransactionType { get; set; }
        public string Notes { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
