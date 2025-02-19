namespace PL.Products.API.Entities;

public class PortfolioPerformance : Entity<Guid>, IAggregateRoot
{
    public int Year { get; set; }
    public decimal NetReturn { get; set; } // Net return as a percentage
    public decimal BenchmarkReturn { get; set; } // Benchmark return as a percentage
    public decimal AssetsInMillions { get; set; } // Portfolio value in millions
    public decimal StandardDeviation { get; set; } // Standard deviation of the returns
}

