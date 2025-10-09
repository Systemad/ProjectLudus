namespace CatalogAPI.Seeding;

public interface ISeederService
{
    Task BeginSeedAsync(CancellationToken cancellationToken);
}
