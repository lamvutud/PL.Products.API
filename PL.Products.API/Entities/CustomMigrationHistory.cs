namespace PL.Products.API.Entities;

public class CustomMigrationHistory : Entity<Guid>
{
    public string MigrationName { get; set; }
}
