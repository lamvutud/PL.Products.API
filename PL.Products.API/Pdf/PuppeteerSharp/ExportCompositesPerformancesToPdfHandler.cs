using System.Reflection;
using Bogus.Bson;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        //BrowserFetcherOptions bfo = new BrowserFetcherOptions()
        //{
        //    Path = Path.GetTempPath(),
        //    Browser = SupportedBrowser.Chrome
        //};
        //var download = await new BrowserFetcher(bfo).DownloadAsync();
        await new BrowserFetcher().DownloadAsync();

        var options = new LaunchOptions()
        {
            Headless = true,
        };

        // Launch the browser and open a new page
        await using var browser = await Puppeteer.LaunchAsync(options);
        await using var page = await browser.NewPageAsync();

        // Set content for the page
        await page.SetContentAsync(html);

        // 2. Replace placeholders with actual content (e.g., disclaimer, footer)
        var footerTemplate = await File.ReadAllTextAsync("./Templates/_Footer.cshtml");
        var headerTemplate = await File.ReadAllTextAsync("./Templates/_Header.cshtml");
        var disclaimerHtml = await File.ReadAllTextAsync("./Templates/_Disclaimer.cshtml");
        var disclosureHtml = await File.ReadAllTextAsync("./Templates/_Disclosures.cshtml");
        var replacePlaceholderFunction = await File.ReadAllTextAsync("./Templates/ReplacePlaceHolder.js");

        await page.EvaluateFunctionAsync(replacePlaceholderFunction, "#disclaimer-placeholder", disclaimerHtml);
        await page.EvaluateFunctionAsync(replacePlaceholderFunction, "#closure-placeholder", disclosureHtml);


        // Wait for key headers to load (ensure the page is rendered before proceeding)
        await page.WaitForSelectorAsync("h1, h2, h3");

        //await page.PdfDataAsync(new PdfOptions
        //{
        //    PrintBackground = true,           
        //    Format = PaperFormat.A4            
        //});

        // 1. Build the Table of Contents (TOC)
        var buildTOCFunction = await File.ReadAllTextAsync("./Templates/BuildTOC.js");
        await page.EvaluateFunctionAsync(buildTOCFunction);

        // Wait for TOC to be created (ensure elements are in place)
        await page.WaitForFunctionAsync(@"() => document.querySelectorAll('.toc-entry').length > 0");


        // 3. Wait for the page to fully render (this might be necessary for complex layouts)
        await page.WaitForSelectorAsync("#toc-list");
        await page.WaitForTimeoutAsync(10000);

        // 4. Get page numbers for each section (after content rendering)
        var getPagesFunction = await File.ReadAllTextAsync("./Templates/GetPages.js");
        var pageNumbers = await page.EvaluateFunctionAsync<Dictionary<string, int>>(getPagesFunction);

        // 5. Append page numbers to the TOC (after page number calculation)
        var tocAppendPagesFunction = await File.ReadAllTextAsync("./Templates/TOCAppendPages.js");
        await page.EvaluateFunctionAsync(tocAppendPagesFunction, pageNumbers);

        // 6. Now, generate the PDF (after finalizing the TOC, page numbers, and other placeholders)
        var bytes = await page.PdfDataAsync(new PdfOptions
        {
            PrintBackground = true,
            DisplayHeaderFooter = true,
            Format = PaperFormat.A4,
            MarginOptions = new MarginOptions { Top = "20px", Bottom = "50px" },
            HeaderTemplate = headerTemplate,
            FooterTemplate = footerTemplate
        });

        // Return the generated PDF bytes
        return bytes;
    }

    public async Task WriteAsync(ExportCompositePerformancesToPdf data, Stream stream)
    {
        using var sw = new BinaryWriter(stream);
        sw.Write(await GetBytesAsync(data));
    }
}
