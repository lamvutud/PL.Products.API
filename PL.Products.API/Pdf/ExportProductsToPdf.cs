using PL.Products.API.Pdf.Pdf;

namespace PL.Products.API.Pdf;

public record ExportProductsToPdf : IPdfRequest
{
    public List<Entities.Product> Products { get; set; }
}
