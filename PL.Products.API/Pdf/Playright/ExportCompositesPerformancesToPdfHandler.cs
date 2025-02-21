using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PL.Products.API.Html;
using PL.Products.API.Pdf.Pdf;

namespace PL.Products.API.Pdf.Playright;

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

        // Launch Playwright browser (Ensure Playwright is installed and browsers are set up)
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        // Set the HTML content
        await page.SetContentAsync(html);

        // Generate PDF
        var pdfBytes = await page.PdfAsync(new()
        {
            Format = "A4",
            PrintBackground = true,
            DisplayHeaderFooter = true
        });

        return pdfBytes;
    }

    public async Task WriteAsync(ExportCompositePerformancesToPdf data, Stream stream)
    {
        using var sw = new BinaryWriter(stream);
        sw.Write(await GetBytesAsync(data));
    }
}
