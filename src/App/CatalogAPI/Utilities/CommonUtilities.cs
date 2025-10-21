using System.Text.Json;
using IGDB.Models;
using Shared.Features;

namespace CatalogAPI.Utilities;

public static class CommonUtilities
{
    public static List<T> GetDistinctEntities<T>(
        IEnumerable<IIdentifier> games,
        Func<IIdentifier, IEnumerable<T>> selector
    )
        where T : class
    {
        return games
            .SelectMany(g => selector(g) ?? Enumerable.Empty<T>())
            //.SelectMany(selector)
            .DistinctBy<T, long>(e => (e as dynamic).Id)
            .ToList();
    }

    private static async Task WriteToJsonCacheAsync<T>(List<T> entities, string path)
    {
        await using var stream = new StreamWriter(path, append: true);
        var options = new JsonSerializerOptions { WriteIndented = false };
        foreach (var json in entities.Select(entity => JsonSerializer.Serialize(entity, options)))
        {
            await stream.WriteLineAsync(json);
        }
    }
}
