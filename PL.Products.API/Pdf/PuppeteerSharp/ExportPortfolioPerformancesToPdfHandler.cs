using PL.Products.API.Html;
using PL.Products.API.Pdf;
using PL.Products.API.Pdf.Pdf;
using PuppeteerSharp;

namespace PL.Products.API.Pdf.PuppeteerSharp;

public class ExportPortfolioPerformancesToPdfHandler : IPdfWriter<ExportPortfolioPerformancesToPdf>
{
    private readonly IHtmlWriter<ExportPortfolioPerformancesToHtml> _htmlWriter;

    public ExportPortfolioPerformancesToPdfHandler(IHtmlWriter<ExportPortfolioPerformancesToHtml> htmlWriter)
    {
        _htmlWriter = htmlWriter;
    }

    public async Task<byte[]> GetBytesAsync(ExportPortfolioPerformancesToPdf data)
    {
        var html = await _htmlWriter.GetStringAsync(new ExportPortfolioPerformancesToHtml { PortfolioPerformances = data.PortfolioPerformances });
        BrowserFetcherOptions bfo = new BrowserFetcherOptions()
        {
            Path = Path.GetTempPath()
        };
        var download = await new BrowserFetcher(bfo).DownloadAsync();
        var options = new LaunchOptions()
        {
            Headless = true,
            ExecutablePath = download.GetExecutablePath(),
        };
        await using var browser = await Puppeteer.LaunchAsync(options);
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);
        //await page.WaitForSelectorAsync("#performanceChart");  // Assuming the chart has an ID of "performanceChart"
        //await page.WaitForTimeoutAsync(2000);  // Optional: wait for an additional 2 seconds to ensure the chart is fully rendered
        //page.Console += (sender, e) =>
        //{
        //    Console.WriteLine("Console log: " + e.Message);
        //};
        var bytes = await page.PdfDataAsync(new PdfOptions
        {
            PrintBackground = true,
        });

        return bytes;
    }

    public async Task WriteAsync(ExportPortfolioPerformancesToPdf data, Stream stream)
    {
        using var sw = new BinaryWriter(stream);
        sw.Write(await GetBytesAsync(data));
    }
}
