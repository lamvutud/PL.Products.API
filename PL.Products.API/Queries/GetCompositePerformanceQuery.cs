using PL.Products.API.Entities;
using PL.Products.API.Repositories;

namespace PL.Products.API.Queries;

public class GetCompositePerformanceQuery : IQuery<List<Entities.CompositePerformance>>
{
}

public class GetCompositePerformanceQueryHandler : IQueryHandler<GetCompositePerformanceQuery, List<Entities.CompositePerformance>>
{
    private readonly ICompositePerformanceRepository _compositePerformanceRepository;

    public GetCompositePerformanceQueryHandler(ICompositePerformanceRepository compositePerformanceRepository)
    {
        this._compositePerformanceRepository = compositePerformanceRepository;
    }

    public Task<List<Entities.CompositePerformance>> HandleAsync(GetCompositePerformanceQuery query, CancellationToken cancellationToken = default)
        => _compositePerformanceRepository.ToListAsync();  
}
