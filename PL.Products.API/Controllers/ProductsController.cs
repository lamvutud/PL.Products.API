using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Products.API.Pdf;
using PL.Products.API.Pdf.Pdf;
using PL.Products.API.Queries;
using System.Net.Mime;

namespace PL.Products.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly Dispatcher _dispatcher;
    private readonly ILogger _logger;
    private readonly IPdfWriter<ExportProductsToPdf> _pdfWriter;


    public ProductsController(Dispatcher dispatcher,
        ILogger<ProductsController> logger,
        IPdfWriter<ExportProductsToPdf> pdfWriter)
    {
        _dispatcher = dispatcher;
        _logger = logger;
        _pdfWriter = pdfWriter;
    }


    [HttpGet("exportaspdf")]
    public async Task<IActionResult> ExportAsPdf()
    {
        var products = await _dispatcher.DispatchAsync(new GetProductsQuery());
        var bytes = await _pdfWriter.GetBytesAsync(new ExportProductsToPdf { Products = products });
        return File(bytes, MediaTypeNames.Application.Octet, "Products.pdf");
    }

}