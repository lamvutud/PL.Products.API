using System.Reflection;
using PL.Products.API.Html;
using PL.Products.API.Pdf;
using PL.Products.API.Pdf.DinkToPdf;
using PL.Products.API.Pdf.Pdf;
//using PL.Products.API.Pdf.PuppeteerSharp;

//using PL.Products.API.Pdf.Syncfusion;

//using PL.Products.API.Pdf.PuppeteerSharp;
using PL.Products.API.Repositories;

namespace PL.Products.API;

public static class ProductModuleServiceCollectionExtensions
{
    public static IServiceCollection AddProductModule(this IServiceCollection services)
    {
        services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
        services.AddScoped(typeof(IPortfolioPerformanceRepository), typeof(PortfolioPerformanceRepository));
        services.AddScoped(typeof(ICompositePerformanceRepository), typeof(CompositePerformanceRepository));
        services.AddMessageHandlers(Assembly.GetExecutingAssembly());
        services.AddScoped<IHtmlWriter<ExportProductsToHtml>, ExportProductsToHtmlHandler>();
        //services.AddScoped<IPdfWriter<ExportProductsToPdf>, ExportProductsToPdfHandler>();
        services.AddScoped<IHtmlWriter<ExportPortfolioPerformancesToHtml>, ExportPortfolioPerformancesToHtmlHandler>();
        services.AddScoped<IPdfWriter<ExportPortfolioPerformancesToPdf>, ExportPortfolioPerformancesToPdfHandler>();
        services.AddScoped<IHtmlWriter<ExportCompositePerformancesToHtml>, ExportCompositePerformancesToHtmlHandler>();
        services.AddScoped<IPdfWriter<ExportCompositePerformancesToPdf>, ExportCompositePerformancesToPdfHandler>();        
        return services;
    }




}
