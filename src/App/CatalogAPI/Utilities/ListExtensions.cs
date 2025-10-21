using IGDB.Models;

namespace CatalogAPI.Utilities;

public static class ListExtensions
{
    public static List<T> Dedoup<T>(this IEnumerable<T> entities)
        where T : IIdentifier
    {
        return entities
            .GroupBy(g => g.Id)
            .Select(g => g.First())
            .ToList();
    }
}
