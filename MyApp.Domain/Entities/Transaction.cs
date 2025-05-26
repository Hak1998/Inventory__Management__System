using MyApp.Domain.Common;

namespace MyApp.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
        public string TransactionType { get; set; }
        public string Notes { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
