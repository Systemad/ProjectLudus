using Shared.Features;
using SyncService.Services;

namespace SyncService.Seed;

public class SyncGames
{
    private ApiClient _apiClient;
    private GameDatabaseService _gameDatabaseService;

    public SyncGames(ApiClient apiClient, GameDatabaseService gameDatabaseService)
    {
        _apiClient = apiClient;
        _gameDatabaseService = gameDatabaseService;
    }

    public async Task FetchAllGamesFromIGDBAsync()
    {
        var countResponse = await _apiClient.FetchGamesCountAsync();

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        var allGames = new List<IGDBGame>();

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            var games = await _apiClient.FetchBatchAsync(itemsToTake, offset);
            await _gameDatabaseService.InsertGameBatchAsync(games);
            allGames.AddRange(games);
            Console.WriteLine(
                $"Batch {i + 1}/{iterations} — Fetched and stored {games.Count} games. Items: {itemsToTake}, Offset: {offset}"
            );
            await Task.Delay(200);
        }

        await _gameDatabaseService.InsertFiltersAsync(allGames);

        /*
        if (writeToCache)
        {
            await WriteToJsonCacheAsync(allGames);
        }
        */
    }
}