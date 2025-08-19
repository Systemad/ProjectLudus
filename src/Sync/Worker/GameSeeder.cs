using System.Text.Json;
using Marten;
using Shared.Features;

namespace Worker;

public class GameSeeder
{
    private readonly IDocumentStore _store;
    private ApiClient _apiClient;

    private const string gameFile = "Cache/gamefile.json";
    private const string JsonFilePath = "Cache/gamefile.json";

    public GameSeeder(IDocumentStore store, ApiClient apiClient)
    {
        _store = store;
        _apiClient = apiClient;
    }

    public async Task PopulateGamesAsync(bool reset = false, bool useCaching = false, bool writeCache = false)
    {
        if (reset)
        {
            await _store.Advanced.Clean.CompletelyRemoveAllAsync();
            await _store.Advanced.Clean.DeleteAllDocumentsAsync();
            await _store.Advanced.Clean.DeleteAllEventDataAsync();
        }

        if (useCaching)
        {
            await SeedFromCacheAsync();
        }
        else
        {
            await SeedFromIGDBAsync(writeCache);
        }
    }
    public async Task SeedFromCacheAsync()
    {
        await using var fs = File.OpenRead(JsonFilePath);

        var games = await OptimizedList.ReadFromStreamAsync(fs);

        var batchSize = 500;
        var batch = new List<IGDBGameRaw>(batchSize);

        foreach (var game in games)
        {
            batch.Add(game);

            if (batch.Count >= batchSize)
            {
                await InsertGamesBatchAsync(batch);
                batch.Clear();
            }
        }

        if (batch.Count > 0)
            await InsertGamesBatchAsync(batch);
    }
    public async Task SeedFromIGDBAsync(bool writeToCache = false)
    {
        var countResponse = await _apiClient.FetchGamesCountAsync();

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        var allGames = new List<IGDBGameRaw>();

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            var games = await _apiClient.FetchBatchAsync(itemsToTake, offset);
            await InsertGamesBatchAsync(games);
            //var games = await InsertGamesBatchAsync(itemsToTake, offset);
            allGames.AddRange(games);
            Console.WriteLine(
                $"Batch {i + 1}/{iterations} — Fetched and stored {games.Count} games. Items: {itemsToTake}, Offset: {offset}"
            );
            await Task.Delay(200);
        }

        if (writeToCache)
        {
            await WriteToJsonCacheAsync(allGames);
        }
    }

    
    private async Task InsertGamesBatchAsync(
        List<IGDBGameRaw> games
    )
    {
        var inserData = new InsertData();
        var flattened = games.NormalizeGames();
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
        
        inserData.GameModes.Clear();
        inserData.Genres.Clear();
        inserData.Platforms.Clear();
        inserData.PlayerPerspectives.Clear();
        inserData.GameEngines.Clear();
        inserData.Themes.Clear();
        inserData.Franchises.Clear();
        inserData.Keywords.Clear();
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

    private static async Task WriteToJsonCacheAsync(List<IGDBGameRaw> games)
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
