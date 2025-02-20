﻿using PL.Products.API.Html;
using PL.Products.API.Pdf.Pdf;
using PuppeteerSharp;

namespace PL.Products.API.Pdf.PuppeteerSharp;

public class ExportProductsToPdfHandler : IPdfWriter<ExportProductsToPdf>
{
    private readonly IHtmlWriter<ExportProductsToHtml> _htmlWriter;

    public ExportProductsToPdfHandler(IHtmlWriter<ExportProductsToHtml> htmlWriter)
    {
        _htmlWriter = htmlWriter;
    }

    public async Task<byte[]> GetBytesAsync(ExportProductsToPdf data)
    {
        var html = await _htmlWriter.GetStringAsync(new ExportProductsToHtml { Products = data.Products });
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);

        var bytes = await page.PdfDataAsync(new PdfOptions
        {
            PrintBackground = true,
        });

        return bytes;
    }

    public async Task WriteAsync(ExportProductsToPdf data, Stream stream)
    {
        using var sw = new BinaryWriter(stream);
        sw.Write(await GetBytesAsync(data));
    }
}
