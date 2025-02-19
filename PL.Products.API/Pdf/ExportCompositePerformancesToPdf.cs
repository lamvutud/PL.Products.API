using PL.Products.API.Pdf.Pdf;

namespace PL.Products.API.Pdf;

public record ExportCompositePerformancesToPdf : IPdfRequest
{
    public List<Entities.CompositePerformance> CompositePerformances { get; set; }
}