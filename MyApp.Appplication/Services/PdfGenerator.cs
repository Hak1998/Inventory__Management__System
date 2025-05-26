using iTextSharp.text;
using iTextSharp.text.pdf;
using MyApp.Application.Interfaces;
using MyApp.Domain.Entities;
using Document = iTextSharp.text.Document;

namespace MyApp.Application.Services
{
    public class PdfGenerator : IPdfGenerator
    {
        public byte[] GenerateTransactionPdf(IEnumerable<Transaction> transactions, string title)
        {
            using var memoryStream = new MemoryStream();
            using (var document = new Document(PageSize.A4))
            using (var writer = PdfWriter.GetInstance(document, memoryStream))
            {
                document.Open();

                // Add title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                document.Add(new Paragraph(title, titleFont));
                document.Add(Chunk.NEWLINE);

                // Add table
                var table = new PdfPTable(5);
                table.WidthPercentage = 100;

                // Headers
                table.AddCell("Date");
                table.AddCell("Product");
                table.AddCell("Quantity");
                table.AddCell("Unit Price");
                table.AddCell("Total");

                // Data rows
                foreach (var transaction in transactions)
                {
                    table.AddCell(transaction.TransactionDate.ToString("yyyy-MM-dd"));
                    table.AddCell(transaction.Product?.Name ?? "N/A");
                    table.AddCell(transaction.Quantity.ToString());
                    table.AddCell(transaction.Product?.Price.ToString("C") ?? "N/A");
                    table.AddCell((transaction.Quantity * (transaction.Product?.Price ?? 0)).ToString("C"));
                }

                document.Add(table);
                document.Close();
            }

            return memoryStream.ToArray();
        }
    }
}
