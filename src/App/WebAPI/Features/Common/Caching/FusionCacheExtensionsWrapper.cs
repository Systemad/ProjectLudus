using Marten;
using Shared.Features;
using ZiggyCreatures.Caching.Fusion;

namespace WebAPI.Features.Common.Caching;

public static partial class FusionCacheExtensions
{
    public static async Task<List<T>> GetOrLoadBatchAsync<T>(this IFusionCache cache, IDocumentSession session, IEnumerable<long> ids, string keyPrefix) where T : class, IIdentifiable
    {
        var keys = ids.Select(id => $"{keyPrefix}:{id}").ToArray();

        var result = await cache.GetOrSetBatchAsync<T>(
            keys,
            async (batchKeys, ct) =>
            {
                var extractedIds = batchKeys.Select(k => long.Parse(k.Split(':')[1])).ToList();
                var platforms = await session.LoadManyAsync<T>(ct, extractedIds);
                return platforms.ToDictionary(p => $"{keyPrefix}:{p.Id}", p => p);
            },
            entry =>
            {
                entry.SetDuration(TimeSpan.FromHours(1));
            });

        return result.Values.ToList();
    }
}