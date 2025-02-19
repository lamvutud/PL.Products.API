using PL.Products.API.Entities;

namespace PL.Products.API.Html;

public class GipsReportViewModel
{
    public string ReportTitle { get; set; }
    public string LogoUrl { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public string PerformanceDataJson { get; set; }
    public string DisclaimerText { get; set; }
    public string DisclosuresText { get; set; }

    public List<Entities.CompositePerformance> Performances { get; set; }
}