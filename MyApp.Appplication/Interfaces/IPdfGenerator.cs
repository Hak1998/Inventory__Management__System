using MyApp.Domain.Entities;

namespace MyApp.Application.Interfaces
{
    public interface IPdfGenerator
    {
        byte[] GenerateTransactionPdf(IEnumerable<Transaction> transactions, string title);
    }
}
