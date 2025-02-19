using Bogus;
using PL.Products.API.Entities;

namespace PL.Products.API.Repositories;

public class ProductRepository : IProductRepository
{
    public Task<List<Product>> ToListAsync()
    {

        var faker = new Faker<Product>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Code, f => f.Commerce.ProductName())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.CreatedDateTime, f => f.Date.Past(2)) // Date within the past 2 years
            .RuleFor(p => p.UpdatedDateTime, f => f.Date.Recent(30)); // Date within the last 30 days

        var products = faker.Generate(100); // Generate 50 products
        return Task.FromResult(products);
    }   
}

