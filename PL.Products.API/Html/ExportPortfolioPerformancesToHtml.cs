namespace PL.Products.API.Html;

public record ExportPortfolioPerformancesToHtml : IHtmlRequest
{
    public List<Entities.PortfolioPerformance> PortfolioPerformances { get; set; }
}

