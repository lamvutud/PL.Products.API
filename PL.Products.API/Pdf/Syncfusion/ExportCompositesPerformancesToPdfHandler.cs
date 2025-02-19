using System.Drawing;
using System.IO;
using PL.Products.API.Html;
using PL.Products.API.Pdf;
using PL.Products.API.Pdf.Pdf;
using PuppeteerSharp;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.HtmlToPdf;

namespace PL.Products.API.Pdf.Syncfusion;

public class ExportCompositePerformancesToPdfHandler : IPdfWriter<ExportCompositePerformancesToPdf>
{
    private readonly IHtmlWriter<ExportCompositePerformancesToHtml> _htmlWriter;

    public ExportCompositePerformancesToPdfHandler(IHtmlWriter<ExportCompositePerformancesToHtml> htmlWriter)
    {
        _htmlWriter = htmlWriter;
    }

    public async Task<byte[]> GetBytesAsync(ExportCompositePerformancesToPdf data)
    {
        HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();   
        BlinkConverterSettings settings = new BlinkConverterSettings(); 
        var html = await _htmlWriter.GetStringAsync(new ExportCompositePerformancesToHtml { CompositePerformances = data.CompositePerformances });
        //settings.EnableToc = true;
        //HtmlToPdfToc toc = new HtmlToPdfToc();

        //toc.Title = "HTML to PDF";
        ////Set title alignment.
        //toc.TitleAlignment = PdfTextAlignment.Center;
        ////Create new HTML to PDF Toc Style.
        //HtmlToPdfTocStyle style = new HtmlToPdfTocStyle();
        ////Set background color.

        ////Set font.
        //style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
        ////Set fore color.

        ////Set paddings.
        //style.Padding = new PdfPaddings(5, 5, 5, 5);
        ////Set toc style.
        //toc.TitleStyle = style;
        ////Set toc to webkit settings.
        //toc.StartingPageNumber = 2;
        //settings.Toc = toc;
        
        htmlToPdfConverter.ConverterSettings = settings;

        PdfDocument document = htmlToPdfConverter.Convert(html, "");
        using MemoryStream stream = new MemoryStream();
        document.Save(stream);
        return stream.ToArray();
    }

    public async Task WriteAsync(ExportCompositePerformancesToPdf data, Stream stream)
    {
        using var sw = new BinaryWriter(stream);
        sw.Write(await GetBytesAsync(data));
    }

  
}
