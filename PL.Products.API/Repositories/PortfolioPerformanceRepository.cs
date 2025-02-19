using Bogus;
using PL.Products.API.Entities;

namespace PL.Products.API.Repositories;

public class PortfolioPerformanceRepository : IPortfolioPerformanceRepository
{
    public Task<List<PortfolioPerformance>> ToListAsync()
    {
        var startYear = DateTime.Now.Year - 10; // Generate performance for the last 10 years
        var faker = new Faker<PortfolioPerformance>()
            .RuleFor(p => p.Year, f => startYear + f.IndexFaker)
            .RuleFor(p => p.NetReturn, f => f.Random.Decimal(-5, 25)) // Simulate performance in range of -5% to 25%
            .RuleFor(p => p.BenchmarkReturn, f => f.Random.Decimal(-5, 20)) // Simulate benchmark returns in range of -5% to 20%
            .RuleFor(p => p.AssetsInMillions, f => f.Finance.Amount(10, 500)) // Portfolio size in millions (between $10M and $500M)
            .RuleFor(p => p.StandardDeviation, f => f.Random.Decimal(5, 20)); // Standard deviation for volatility (5% to 20%)

        var portfolioPerformance = faker.Generate(100); // Generate 10 years of performance data
        return Task.FromResult(portfolioPerformance);
    }
}

