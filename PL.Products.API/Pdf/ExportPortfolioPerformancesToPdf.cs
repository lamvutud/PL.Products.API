using PL.Products.API.Pdf.Pdf;

namespace PL.Products.API.Pdf;

public record ExportPortfolioPerformancesToPdf : IPdfRequest
{
    public List<Entities.PortfolioPerformance> PortfolioPerformances { get; set; }
}
