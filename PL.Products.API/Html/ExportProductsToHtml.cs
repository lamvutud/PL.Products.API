using System.Collections.Generic;

namespace PL.Products.API.Html;

public record ExportProductsToHtml : IHtmlRequest
{
    public List<Entities.Product> Products { get; set; }
}

