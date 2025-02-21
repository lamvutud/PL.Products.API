using PL.Products.API.Html;
using PL.Products.API.Pdf.Pdf;
using PuppeteerSharp;
using PuppeteerSharp.Media;

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
        var buildTOCFunction = await File.ReadAllTextAsync("./Templates/BuildTOC.js");
        await page.EvaluateFunctionAsync(buildTOCFunction);
        await page.WaitForFunctionAsync(@"() => document.querySelectorAll('.toc-entry').length > 0");
        await page.PdfDataAsync(new PdfOptions
        {
            PrintBackground = true           
        });
        var getPagesFunction = await File.ReadAllTextAsync("./Templates/GetPages.js");
        var pageNumbers = await page.EvaluateFunctionAsync<Dictionary<string, int>>(getPagesFunction);
        var tocAppendPagesFunction = await File.ReadAllTextAsync("./Templates/TOCAppendPages.js");
        await page.EvaluateFunctionAsync(tocAppendPagesFunction, pageNumbers);
        var footerTemplate = await File.ReadAllTextAsync("./Templates/_Footer.cshtml");
        var headerTemplate = await File.ReadAllTextAsync("./Templates/_Header.cshtml");
        var bytes = await page.PdfDataAsync(new PdfOptions
        {
            PrintBackground = true,
            DisplayHeaderFooter = true,
            MarginOptions = new MarginOptions { Top = "20px", Bottom = "50px" },
            HeaderTemplate = headerTemplate,
            FooterTemplate = footerTemplate
        });

        return bytes;
    }

    public async Task WriteAsync(ExportCompositePerformancesToPdf data, Stream stream)
    {
        using var sw = new BinaryWriter(stream);
        sw.Write(await GetBytesAsync(data));
    }
}
