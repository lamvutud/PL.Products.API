namespace PL.Products.API.Entities;

public interface IHasKey<T>
{
    T Id { get; set; }
}
