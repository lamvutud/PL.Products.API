using Newtonsoft.Json;
using RazorLight;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PL.Products.API.Html;

public class ExportPortfolioPerformancesToHtmlHandler : IHtmlWriter<ExportPortfolioPerformancesToHtml>
{
    private readonly IRazorLightEngine _razorLightEngine;

    public ExportPortfolioPerformancesToHtmlHandler(IRazorLightEngine razorLightEngine)
    {
        _razorLightEngine = razorLightEngine;
    }

    public async Task WriteAsync(ExportPortfolioPerformancesToHtml data, Stream stream)
    {
        using var sw = new StreamWriter(stream, Encoding.UTF8);
        await sw.WriteAsync(await GetStringAsync(data));
    }

    public async Task<string> GetStringAsync(ExportPortfolioPerformancesToHtml data)
    {
        var template = Path.Combine(Environment.CurrentDirectory, $"Templates/PortfolioPerformance.cshtml");

        var performanceDataJson = JsonConvert.SerializeObject(data.PortfolioPerformances.Select(p => new
        {
            p.Year,
            p.NetReturn,
            p.BenchmarkReturn,
            p.AssetsInMillions,
            p.StandardDeviation
        }).ToList());

        var templateData = new PortfolioPerformanceViewModel
        {
            PortfolioPerformances = data.PortfolioPerformances,
            PerformanceDataJson = performanceDataJson
        };

        string html = await _razorLightEngine.CompileRenderAsync(template, templateData);
        return html;
    }
}
