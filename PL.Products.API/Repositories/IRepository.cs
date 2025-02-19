using PL.Products.API.Entities;

namespace PL.Products.API.Repositories;

public interface IRepository<TEntity, TKey>
    where TEntity : Entity<TKey>, IAggregateRoot
{
    Task<List<TEntity>> ToListAsync();
}
