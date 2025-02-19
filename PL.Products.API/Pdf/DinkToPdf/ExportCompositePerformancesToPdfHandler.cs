using DinkToPdf;
using DinkToPdf.Contracts;
using PL.Products.API.Html;
using PL.Products.API.Pdf.Pdf;

namespace PL.Products.API.Pdf.DinkToPdf;

public class ExportCompositePerformancesToPdfHandler : IPdfWriter<ExportCompositePerformancesToPdf>
{
    private readonly IConverter _converter;
    private readonly IHtmlWriter<ExportCompositePerformancesToHtml> _htmlWriter;

    public ExportCompositePerformancesToPdfHandler(IConverter converter, IHtmlWriter<ExportCompositePerformancesToHtml> htmlWriter)
    {
        _converter = converter;
        _htmlWriter = htmlWriter;
    }

    public async Task<byte[]> GetBytesAsync(ExportCompositePerformancesToPdf data)
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
                    HtmlContent = await _htmlWriter.GetStringAsync(new ExportCompositePerformancesToHtml {CompositePerformances = data.CompositePerformances}),
                    WebSettings = {
                        EnableJavascript = true,
                        LoadImages = true,
                        PrintMediaType = true },
                    HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                    UseExternalLinks = true,
                    UseLocalLinks = true,
                    LoadSettings = new LoadSettings() {JSDelay = 3000}
                },
            },
        };

        var bytes = _converter.Convert(doc);
        return bytes;
    }

    public async Task WriteAsync(ExportCompositePerformancesToPdf data, Stream stream)
    {
        using var sw = new BinaryWriter(stream);
        sw.Write(await GetBytesAsync(data));
    }
}
