using System.Text.Json;
using Shared.Features;
using SyncService.Cache;

namespace SyncService.Utilities;

public static class CommonUtilities
{
    public static List<T> GetDistinctEntities<T>(
        IEnumerable<IGDBGame> games,
        Func<IGDBGame, IEnumerable<T>> selector
    )
        where T : class
    {
        return games
            .SelectMany(g => selector(g) ?? Enumerable.Empty<T>())
            //.SelectMany(selector)
            .DistinctBy<T, long>(e => (e as dynamic).Id) // assuming Id is int
            .ToList();
    }
    
    private static async Task WriteToJsonCacheAsync(List<IGDBGame> games)
    {
        await using var stream = new StreamWriter(FilePath.GAMES, append: true);
        var options = new JsonSerializerOptions { WriteIndented = false };
        foreach (var json in games.Select(game => JsonSerializer.Serialize(game, options)))
        {
            await stream.WriteLineAsync(json);
        }
    }
}