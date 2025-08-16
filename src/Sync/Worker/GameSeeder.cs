using System.Text.Json;
using Marten;
using Shared.Features;

namespace Worker;

public class GameSeeder
{
    private readonly IDocumentStore _store;
    private ApiClient _apiClient;

    private const string gameFile = "Cache/gamefile.json";

    public GameSeeder(IDocumentStore store, ApiClient apiClient)
    {
        _store = store;
        _apiClient = apiClient;
    }

    public async Task PopulateGamesAsync(bool writeToCache = false, bool reset = false)
    {
        if (reset)
        {
            await _store.Advanced.Clean.CompletelyRemoveAllAsync();
            await _store.Advanced.Clean.DeleteAllDocumentsAsync();
            await _store.Advanced.Clean.DeleteAllEventDataAsync();
        }

        var countResponse = await _apiClient.FetchGamesCountAsync();

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        var allGames = new List<IgdbGame>();

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            var games = await InsertGamesBatchAsync(itemsToTake, offset);
            allGames.AddRange(games);
            Console.WriteLine(
                $"Batch {i + 1}/{iterations} — Fetched and stored {games.Count} games. Items: {itemsToTake}, Offset: {offset}"
            );
            await Task.Delay(200);
        }

        //var gameTypes = await _apiClient.FetchGamesTypesAsync();
        //session.StoreObjects(gameTypes);
        if (writeToCache)
        {
            await WriteToJsonCacheAsync(allGames);
        }
    }

    private async Task<List<IgdbGame>> InsertGamesBatchAsync(
        long itemsToTake,
        long offset
    )
    {
        var inserData = new InsertData();
        var games = await _apiClient.FetchBatchAsync(itemsToTake, offset);
        var flattened = games.FlattenGames();
        await _store.BulkInsertAsync(flattened, BulkInsertMode.OverwriteExisting);

        inserData.GameModes.AddRange(Utilities.GetDistinctEntities(games, g => g.GameModes));
        inserData.Genres.AddRange(Utilities.GetDistinctEntities(games, g => g.Genres));
        inserData.Platforms.AddRange(Utilities.GetDistinctEntities(games, g => g.Platforms));
        inserData.PlayerPerspectives.AddRange(Utilities.GetDistinctEntities(games, g => g.PlayerPerspectives));
        inserData.GameEngines.AddRange(Utilities.GetDistinctEntities(games, g => g.GameEngines));
        inserData.Themes.AddRange(Utilities.GetDistinctEntities(games, g => g.Themes));
        inserData.Franchises.AddRange(Utilities.GetDistinctEntities(games, g => g.Franchises));
        inserData.Keywords.AddRange(Utilities.GetDistinctEntities(games, g => g.Keywords));
        await InsertDataAsync(inserData);
        return games;
    }

    private async Task InsertDataAsync(InsertData insertData)
    {
        await _store.BulkInsertAsync(insertData.GameModes, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Genres, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Platforms, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(
            insertData.PlayerPerspectives,
            BulkInsertMode.OverwriteExisting
        );
        await _store.BulkInsertAsync(insertData.GameEngines, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Themes, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Franchises, BulkInsertMode.OverwriteExisting);
        await _store.BulkInsertAsync(insertData.Keywords, BulkInsertMode.OverwriteExisting);
    }

    private static async Task WriteToJsonCacheAsync(List<IgdbGame> games)
    {
        await using var stream = new StreamWriter(gameFile, append: true);
        var options = new JsonSerializerOptions { WriteIndented = false };
        foreach (var game in games)
        {
            string json = JsonSerializer.Serialize(game, options);
            await stream.WriteLineAsync(json);
        }
    }


    private static List<T> GetDistinctEntities2<T>(IEnumerable<T> items) where T : class
    {
        return items
            .DistinctBy(e => (e as dynamic).Id)
            .ToList();
    }
}
