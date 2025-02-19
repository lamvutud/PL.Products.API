namespace PL.Products.API.Html;

public record ExportCompositePerformancesToHtml : IHtmlRequest
{
    public List<Entities.CompositePerformance> CompositePerformances { get; set; }
}

