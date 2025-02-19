using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Products.API.Pdf;
using PL.Products.API.Pdf.Pdf;
using PL.Products.API.Queries;
using System.Net.Mime;

namespace PL.Products.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompositesController : ControllerBase
{
    private readonly Dispatcher _dispatcher;
    private readonly ILogger _logger;
    private readonly IPdfWriter<ExportCompositePerformancesToPdf> _pdfWriter;


    public CompositesController(Dispatcher dispatcher,
        ILogger<CompositesController> logger,
        IPdfWriter<ExportCompositePerformancesToPdf> pdfWriter)
    {
        _dispatcher = dispatcher;
        _logger = logger;
        _pdfWriter = pdfWriter;
    }


    [HttpGet("exportaspdf")]
    public async Task<IActionResult> ExportAsPdf()
    {
        var compositePerformances = await _dispatcher.DispatchAsync(new GetCompositePerformanceQuery());
        var bytes = await _pdfWriter.GetBytesAsync(new ExportCompositePerformancesToPdf { CompositePerformances = compositePerformances });
        return File(bytes, MediaTypeNames.Application.Octet, "Composites.pdf");
    }

}