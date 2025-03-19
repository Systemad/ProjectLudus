namespace Ludus.Service;

public interface IDataSeeder
{
    Task Seed();
    Task FetchAndStoreGamesAsync(long itemsToTake, long offset);
}
