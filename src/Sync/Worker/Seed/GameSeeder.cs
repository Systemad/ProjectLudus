using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;
using Shared.Features;
using Worker.Data;

namespace Worker.Seed;

public class GameSeeder
{
    private ApiClient _apiClient;

    private const string gameFile = "Cache/gamefile.json";
    private const string JsonFilePath = "Cache/gamefile.json";
    private AppDbContext _context;

    public GameSeeder(ApiClient apiClient, AppDbContext context)
    {
        _apiClient = apiClient;
        _context = context;
    }

    public async Task PopulateGamesAsync(bool reset = false, bool useCaching = false, bool writeCache = false)
    {
        if (reset)
        {
        }

        if (useCaching)
        {
            await SeedFromCacheAsync();
        }
        else
        {
            await SeedFromIgdbAsync(writeCache);
        }
    }

    private async Task SeedFromCacheAsync()
    {
        await using var fs = File.OpenRead(JsonFilePath);
        var games = await OptimizedList.ReadFromStreamAsync(fs);
        await InsertGameBatchAsync(games);
        await InsertFiltersAsync(games);

        await _context.SaveChangesAsync();
    }
    
    public async Task SeedFromIgdbAsync(bool writeToCache = false)
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
            await InsertGameBatchAsync(games);
            //var games = await InsertGamesBatchAsync(itemsToTake, offset);
            allGames.AddRange(games);
            Console.WriteLine(
                $"Batch {i + 1}/{iterations} — Fetched and stored {games.Count} games. Items: {itemsToTake}, Offset: {offset}"
            );
            await Task.Delay(200);
        }

        await InsertFiltersAsync(allGames);

        if (writeToCache)
        {
            await WriteToJsonCacheAsync(allGames);
        }
    }


    private async Task InsertGameBatchAsync(List<IGDBGame> games)
    {
        var entities = games.Select(g => new GameEntity
        {
            Id = 0,
            Name = g.Name,
            ReleaseDate = Instant.FromUnixTimeSeconds(g.FirstReleaseDate),
            GameType = g.GameType.Id,
            Platforms = g.Platforms?.Select(x => x.Id).ToArray() ?? [],
            GameEngines = g.GameEngines?.Select(x => x.Id).ToArray() ?? [],
            Genres = g.Genres?.Select(x => x.Id).ToArray() ?? [],
            Themes = g.Themes?.Select(x => x.Id).ToArray() ?? [],
            Rating = g.Rating ?? 0,
            RatingCount = g.RatingCount ?? 0,
            TotalRating = g.TotalRating ?? 0,
            TotalRatingCount = g.TotalRatingCount ?? 0,
            UpdatedAt = g.UpdatedAt,
            RawData = g
        }).ToList();

        await _context.Games.ExecuteBulkInsertAsync(entities);
    }
    

    private async Task InsertFiltersAsync(
        List<IGDBGame> games
    )
    {
        var inserData = new InsertData();
        inserData.GameModes.AddRange(Utilities.GetDistinctEntities(games, g => g.GameModes));
        inserData.Genres.AddRange(Utilities.GetDistinctEntities(games, g => g.Genres));
        inserData.Platforms.AddRange(Utilities.GetDistinctEntities(games, g => g.Platforms));
        inserData.PlayerPerspectives.AddRange(Utilities.GetDistinctEntities(games, g => g.PlayerPerspectives));
        inserData.GameEngines.AddRange(Utilities.GetDistinctEntities(games, g => g.GameEngines));
        inserData.Themes.AddRange(Utilities.GetDistinctEntities(games, g => g.Themes));
        inserData.Franchises.AddRange(Utilities.GetDistinctEntities(games, g => g.Franchises));
        inserData.Keywords.AddRange(Utilities.GetDistinctEntities(games, g => g.Keywords));
        
        await BulkInsertInBatchesAsync(inserData.GameModes, _context.GameModes);
        await BulkInsertInBatchesAsync(inserData.Genres, _context.Genres);
        await BulkInsertInBatchesAsync(inserData.Platforms, _context.Platforms);
        await BulkInsertInBatchesAsync(inserData.PlayerPerspectives, _context.PlayerPerspectives);
        await BulkInsertInBatchesAsync(inserData.GameEngines, _context.GameEngines);
        await BulkInsertInBatchesAsync(inserData.Themes, _context.Themes);
        await BulkInsertInBatchesAsync(inserData.Franchises, _context.Franchises);
        await BulkInsertInBatchesAsync(inserData.Keywords, _context.Keywords);
        
    }


    private async Task BulkInsertInBatchesAsync<T>(
        IEnumerable<T> items,
        DbSet<T> dbSet,
        int batchSize = 10_000
    ) where T : class
    {
        foreach (var batch in items.Chunk(batchSize))
        {
            await dbSet.ExecuteBulkInsertAsync(batch);
        }
    }

    
    private static async Task WriteToJsonCacheAsync(List<IGDBGame> games)
    {
        await using var stream = new StreamWriter(gameFile, append: true);
        var options = new JsonSerializerOptions { WriteIndented = false };
        foreach (var json in games.Select(game => JsonSerializer.Serialize(game, options)))
        {
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