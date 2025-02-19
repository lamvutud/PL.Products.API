using PL.Products.API.Repositories;

namespace PL.Products.API.Queries;

public class GetProductsQuery : IQuery<List<Entities.Product>>
{
}

public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, List<Entities.Product>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<List<Entities.Product>> HandleAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
        => _productRepository.ToListAsync();
}
