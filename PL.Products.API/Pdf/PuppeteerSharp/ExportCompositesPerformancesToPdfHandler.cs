using PL.Products.API.Html;
using PL.Products.API.Pdf;
using PL.Products.API.Pdf.Pdf;
using PuppeteerSharp;

namespace PL.Products.API.Pdf.PuppeteerSharp;

public class ExportCompositePerformancesToPdfHandler : IPdfWriter<ExportCompositePerformancesToPdf>
{
    private readonly IHtmlWriter<ExportCompositePerformancesToHtml> _htmlWriter;

    public ExportCompositePerformancesToPdfHandler(IHtmlWriter<ExportCompositePerformancesToHtml> htmlWriter)
    {
        _htmlWriter = htmlWriter;
    }

    public async Task<byte[]> GetBytesAsync(ExportCompositePerformancesToPdf data)
    {
        var html = await _htmlWriter.GetStringAsync(new ExportCompositePerformancesToHtml { CompositePerformances = data.CompositePerformances });
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
        var bytes = await page.PdfDataAsync(new PdfOptions
        {
            PrintBackground = true,
            DisplayHeaderFooter = true,           

        });

        return bytes;
    }

    public async Task WriteAsync(ExportCompositePerformancesToPdf data, Stream stream)
    {
        using var sw = new BinaryWriter(stream);
        sw.Write(await GetBytesAsync(data));
    }


}
