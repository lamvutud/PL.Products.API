using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Products.API.Pdf;
using PL.Products.API.Pdf.Pdf;
using PL.Products.API.Queries;
using System.Net.Mime;

namespace PL.Products.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PortfoliosController : ControllerBase
{
    private readonly Dispatcher _dispatcher;
    private readonly ILogger _logger;
    private readonly IPdfWriter<ExportPortfolioPerformancesToPdf> _pdfWriter;


    public PortfoliosController(Dispatcher dispatcher,
        ILogger<PortfoliosController> logger,
        IPdfWriter<ExportPortfolioPerformancesToPdf> pdfWriter)
    {
        _dispatcher = dispatcher;
        _logger = logger;
        _pdfWriter = pdfWriter;
    }


    [HttpGet("exportaspdf")]
    public async Task<IActionResult> ExportAsPdf()
    {
        var portfolioPerformances = await _dispatcher.DispatchAsync(new GetPortfolioPerformanceQuery());
        var bytes = await _pdfWriter.GetBytesAsync(new ExportPortfolioPerformancesToPdf { PortfolioPerformances = portfolioPerformances });
        return File(bytes, MediaTypeNames.Application.Octet, "Portfolios.pdf");
    }

}