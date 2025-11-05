namespace CatalogAPI.Seeding;

public interface IDataFetcherService
{
    Task FetchDataAsync(CancellationToken cancellationToken);
    Task<bool> ParquetExistsAsync(CancellationToken cancellationToken);
}
