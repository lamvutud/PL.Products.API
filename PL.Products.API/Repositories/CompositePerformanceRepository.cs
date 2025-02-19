using PL.Products.API.Entities;

namespace PL.Products.API.Repositories;

public class CompositePerformanceRepository : ICompositePerformanceRepository
{
    public Task<List<CompositePerformance>> ToListAsync()
    {
        return Task.FromResult(new List<CompositePerformance>([new CompositePerformance { Year = 2024, Return = 8.5m, BenchmarkReturn = 7.9m, IsPartialYear = true },
                new CompositePerformance { Year = 2023, Return = 12.3m, BenchmarkReturn = 11.5m, IsPartialYear = false },
                new CompositePerformance { Year = 2022, Return = -5.0m, BenchmarkReturn = -4.8m, IsPartialYear = false },
                new CompositePerformance { Year = 2021, Return = 15.6m, BenchmarkReturn = 14.9m, IsPartialYear = false },
                new CompositePerformance { Year = 2020, Return = 10.2m, BenchmarkReturn = 9.7m, IsPartialYear = false },
                new CompositePerformance { Year = 2019, Return = 8.9m, BenchmarkReturn = 8.2m, IsPartialYear = false },
                new CompositePerformance { Year = 2018, Return = 4.3m, BenchmarkReturn = 3.8m, IsPartialYear = false },
                new CompositePerformance { Year = 2017, Return = 13.1m, BenchmarkReturn = 12.5m, IsPartialYear = false },
                new CompositePerformance { Year = 2016, Return = 9.0m, BenchmarkReturn = 8.5m, IsPartialYear = false },
                new CompositePerformance { Year = 2015, Return = 7.2m, BenchmarkReturn = 6.9m, IsPartialYear = false }]));
    }
}

