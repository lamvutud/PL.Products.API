﻿using DinkToPdf;
using DinkToPdf.Contracts;
using PL.Products.API.Html;
using PL.Products.API.Pdf.Pdf;

namespace PL.Products.API.Pdf.DinkToPdf;

public class ExportPortfolioPerformancesToPdfHandler : IPdfWriter<ExportPortfolioPerformancesToPdf>
{
    private readonly IConverter _converter;
    private readonly IHtmlWriter<ExportPortfolioPerformancesToHtml> _htmlWriter;

    public ExportPortfolioPerformancesToPdfHandler(IConverter converter, IHtmlWriter<ExportPortfolioPerformancesToHtml> htmlWriter)
    {
        _converter = converter;
        _htmlWriter = htmlWriter;
    }

    public async Task<byte[]> GetBytesAsync(ExportPortfolioPerformancesToPdf data)
    {
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings =
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10, Bottom = 15, Left = 10, Right = 10 },
            },
            Objects =
            {
                new ObjectSettings()
                {
                    PagesCount = true,
                    HtmlContent = await _htmlWriter.GetStringAsync(new ExportPortfolioPerformancesToHtml {PortfolioPerformances = data.PortfolioPerformances}),
                    WebSettings = { DefaultEncoding = "utf-8", Background = true },
                    HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                },
            },
        };

        var bytes = _converter.Convert(doc);
        return bytes;
    }

    public async Task WriteAsync(ExportPortfolioPerformancesToPdf data, Stream stream)
    {
        using var sw = new BinaryWriter(stream);
        sw.Write(await GetBytesAsync(data));
    }
}
