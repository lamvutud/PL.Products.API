using PL.Products.API.Entities;
using PL.Products.API.Repositories;

namespace PL.Products.API.Queries;

public class GetPortfolioPerformanceQuery : IQuery<List<Entities.PortfolioPerformance>>
{
}

public class GetPortfolioPerformanceQueryHandler : IQueryHandler<GetPortfolioPerformanceQuery, List<Entities.PortfolioPerformance>>
{
    private readonly IPortfolioPerformanceRepository _portfolioPerformanceRepository;

    public GetPortfolioPerformanceQueryHandler(IPortfolioPerformanceRepository portfolioPerformanceRepository)
    {
        this._portfolioPerformanceRepository = portfolioPerformanceRepository;
    }

    public Task<List<Entities.PortfolioPerformance>> HandleAsync(GetPortfolioPerformanceQuery query, CancellationToken cancellationToken = default)
        => _portfolioPerformanceRepository.ToListAsync();  
}
