using System.Text;
using IGDBService.Twitch;
using Marten;
using Shared;
using Shared.Features.Games;

namespace IGDBService;

public class Seeder
{
    private readonly IDocumentStore _store;
    private ApiClient _apiClient;

    public Seeder(IDocumentStore store, ApiClient apiClient)
    {
        _store = store;
        _apiClient = apiClient;
    }

    public async Task PopulateDatabase()
    {
        var countResponse = await _apiClient.FetchGamesCountAsync();

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        await using var session = _store.LightweightSession();

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            Console.WriteLine($"Fetching {itemsToTake} items!");
            await PopulateBatch(itemsToTake, offset);
            await Task.Delay(300);
        }

        var gameTypes = await _apiClient.FetchGamesTypesAsync();
        session.StoreObjects(gameTypes);

        await Task.CompletedTask;
    }

    private async Task PopulateBatch(long itemsToTake, long offset)
    {
        await using var session = _store.LightweightSession();
        var games = await _apiClient.FetchBatchAsync(itemsToTake, offset);

        /*
        var gameModes = GetDistinctEntities(games, g => g.GameModes);
        var genres = GetDistinctEntities(games, g => g.Genres);
        var involvedCompanies = GetDistinctEntities(games, g => g.InvolvedCompanies);
        var platforms = GetDistinctEntities(games, g => g.Platforms);
        var playerPerspectives = GetDistinctEntities(games, g => g.PlayerPerspectives);
        var gameEngines = GetDistinctEntities(games, g => g.GameEngines);
        var themes = GetDistinctEntities(games, g => g.Themes);
        var franchises = GetDistinctEntities(games, g => g.Franchises);

        session.StoreObjects(gameModes);
        session.StoreObjects(genres);
        session.StoreObjects(involvedCompanies);
        session.StoreObjects(platforms);
        session.StoreObjects(playerPerspectives);
        session.StoreObjects(gameEngines);
        session.StoreObjects(themes);
        session.StoreObjects(franchises);
        
        */
        foreach (var item in games)
        {
            session.Store(item.GameModes);
            session.Store(item.Genres);
            session.Store(item.InvolvedCompanies);
            session.Store(item.Platforms);
            session.Store(item.PlayerPerspectives);
            session.Store(item.Themes);
            session.Store(item.GameEngines);
            session.Store(item.Themes);
            session.Store(item.Franchises);
        }

        await session.DocumentStore.BulkInsertAsync(games, BulkInsertMode.OverwriteExisting);
        await session.SaveChangesAsync();
    }

    private static IList<T> GetDistinctEntities<T>(
        IEnumerable<RawGame> games,
        Func<RawGame, IEnumerable<T>> selector
    )
        where T : class
    {
        return games
            .SelectMany(g => selector(g) ?? Enumerable.Empty<T>())
            //.SelectMany(selector)
            .DistinctBy<T, long>(e => (e as dynamic).Id) // assuming Id is int
            .ToList();
    }
}
