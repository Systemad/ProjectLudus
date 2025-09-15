using Shared.Features;
using Shared.Queries;

namespace SyncService.Features.Games;

public class GameSeedingSeedingController(ApiClient apiClient, GameDatabaseService gameDatabaseService)
{
    public async Task RunCatchupSeedingAsync()
    {
        var countResponse = await apiClient.FetchCountAsync(GameQuery.Url);

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        var allGames = new List<IGDBGame>();

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            var games = await apiClient.FetchBatchAsync(itemsToTake, offset);
            await gameDatabaseService.InsertGameBatchAsync(games);
            allGames.AddRange(games);
            Console.WriteLine(
                $"Batch {i + 1}/{iterations} — Fetched and stored {games.Count} games. Items: {itemsToTake}, Offset: {offset}"
            );
            await Task.Delay(200);
        }
        await gameDatabaseService.InsertFiltersAsync(allGames);
    }
    
    public async Task RunInitialSeedingAsync()
    {
        var countResponse = await apiClient.FetchCountAsync(GameQuery.Url);

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        var allGames = new List<IGDBGame>();

        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            var games = await apiClient.FetchBatchAsync(itemsToTake, offset);
            await gameDatabaseService.InsertGameBatchAsync(games);
            allGames.AddRange(games);
            Console.WriteLine(
                $"Batch {i + 1}/{iterations} — Fetched and stored {games.Count} games. Items: {itemsToTake}, Offset: {offset}"
            );
            await Task.Delay(200);
        }

        await gameDatabaseService.InsertFiltersAsync(allGames);

    }
}