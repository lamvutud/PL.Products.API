namespace PL.Products.API.Entities;

public class CompositePerformance:  Entity<Guid>, IAggregateRoot
{
    public int Year { get; set; }
    public decimal Return { get; set; }
    public decimal BenchmarkReturn { get; set; }
    public bool IsPartialYear { get; set; }
}
