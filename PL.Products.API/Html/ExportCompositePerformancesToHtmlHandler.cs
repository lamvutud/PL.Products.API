using Newtonsoft.Json;
using Razor.Templating.Core;
using RazorLight;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PL.Products.API.Html;

public class ExportCompositePerformancesToHtmlHandler : IHtmlWriter<ExportCompositePerformancesToHtml>
{
    private readonly IRazorLightEngine _razorLightEngine;

    public ExportCompositePerformancesToHtmlHandler(IRazorLightEngine razorLightEngine)
    {
        _razorLightEngine = razorLightEngine;
    }

    public async Task WriteAsync(ExportCompositePerformancesToHtml data, Stream stream)
    {
        using var sw = new StreamWriter(stream, Encoding.UTF8);
        await sw.WriteAsync(await GetStringAsync(data));
    }

    public async Task<string> GetStringAsync(ExportCompositePerformancesToHtml data)
    {
        var template = Path.Combine(Environment.CurrentDirectory, $"Templates/CompositePerformance.cshtml");

        var performanceDataJson = JsonConvert.SerializeObject(data.CompositePerformances.Select(p => new
        {
            p.Year,
            p.Return,
            p.BenchmarkReturn,
            p.IsPartialYear            
        }).ToList());

        var templateData = new GipsReportViewModel
        {
            ReportTitle = "GIPS Composite Performance Report",
            LogoUrl = "https://example.com/logo.png",
            CompanyName = "Investment Firm Inc.",
            CompanyAddress = "123 Finance Street, New York, NY",
            PerformanceDataJson = performanceDataJson,
            Performances = data.CompositePerformances,
            DisclaimerText = "This report is for informational purposes only. Past performance is not indicative of future results.",
            DisclosuresText = "All investments involve risk, including the potential loss of principal."
        };

        string html = await _razorLightEngine.CompileRenderAsync(template, templateData);
        return html;
    }
}
