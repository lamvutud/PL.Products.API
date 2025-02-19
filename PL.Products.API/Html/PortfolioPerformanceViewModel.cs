namespace PL.Products.API.Html;

public class PortfolioPerformanceViewModel
{
    public IEnumerable<PL.Products.API.Entities.PortfolioPerformance> PortfolioPerformances { get; set; }
    public string PerformanceDataJson { get; set; }
}
