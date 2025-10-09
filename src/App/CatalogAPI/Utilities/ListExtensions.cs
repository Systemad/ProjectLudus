using Shared.Features;

namespace CatalogAPI.Utilities;

public static class ListExtensions
{
    public static List<T> Dedoup<T>(this IEnumerable<T> entities)
        where T : IgdbResponse
    {
        return entities
            .GroupBy(g => g.Id)
            .Select(g => g.OrderByDescending(x => x.UpdatedAt).First())
            .ToList();
    }
}
