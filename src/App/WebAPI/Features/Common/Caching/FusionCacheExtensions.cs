using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;
using ZiggyCreatures.Caching.Fusion;

namespace WebAPI.Features.Common.Caching;

// https://gist.github.com/invalidoperation/2d48e05c44cf66c9a188d480239fee2b
public static partial class FusionCacheExtensions
{
    /// <summary>
    ///  Get all values of type TValue in the cache for the specified keys grouped as <see cref="IDictionary{TKey,TValue}"/>:
    /// if not there, the factory will be called for all missing and stale keys and then saved according to the options provided.
    /// </summary>
    /// <param name="cache">The <see cref="IFusionCache"/> Instance</param>
    /// <param name="keys">The cache keys which identify the entries in the cache</param>
    /// <param name="factory">The batch updater function. Receives an array of <see cref="string" /> missing and/or stale keys,
    /// and should return an <see cref="IDictionary{TKey,TValue}"/>
    /// of type <see cref="string"/>,<see cref="T" /></param>
	/// <param name="setupAction">The setup action used to further configure the newly created <see cref="FusionCacheEntryOptions"/> object,
	/// automatically created by duplicating <see cref="IFusionCache.DefaultEntryOptions"/>.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <typeparam name="T">The type of the value in the cache</typeparam>
    /// <returns> The values in the cache, grouped as <see cref="IDictionary{TKey,TValue}"/>,
    /// combined with already there and generated values using the provided factory</returns>
    public static async Task<IDictionary<string, T>>    GetOrSetBatchAsync<T>(
        this IFusionCache cache, 
        string[] keys, 
        Func<string[], CancellationToken, Task<IDictionary<string,T>>> factory, 
        Action<FusionCacheEntryOptions>? setupAction = null,
        CancellationToken cancellationToken = default)
        => await cache.GetOrSetBatchAsync(
            keys, 
            factory,
            cache.CreateEntryOptions(setupAction), 
            cancellationToken);

    /// <summary>
    ///  Get all values of type TValue in the cache for the specified keys grouped as <see cref="IDictionary{TKey,TValue}"/>:
    /// if not there, the factory will be called for all missing and stale keys and then saved according to the options provided.
    /// </summary>
    /// <param name="cache">The <see cref="IFusionCache"/> Instance</param>
    /// <param name="keys">The cache keys which identify the entries in the cache</param>
    /// <param name="factory">The batch updater function. Receives an array of <see cref="string" /> missing and/or stale keys,
    /// and should return an <see cref="IDictionary{TKey,TValue}"/>
    /// of type <see cref="string"/>,<see cref="T" /></param>
    /// <param name="options">The options to adhere during this operation when setting the cache.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <typeparam name="T">The type of the value in the cache</typeparam>
    /// <returns> The values in the cache, grouped as <see cref="IDictionary{TKey,TValue}"/>,
    /// combined with already there and generated values using the provided factory</returns>
    public static async Task<IDictionary<string, T>> GetOrSetBatchAsync<T>(
        this IFusionCache cache, 
        string[] keys, 
        Func<string[], CancellationToken, Task<IDictionary<string,T>>> factory, 
        FusionCacheEntryOptions options,
        CancellationToken cancellationToken = default)
    {
        var cachedResults = new ConcurrentDictionary<string, T>();
        var missingKeys = new ConcurrentBag<string>();
        var staleKeys = new ConcurrentBag<string>();

        await Task.WhenAll(keys.Select(async key =>
        {
            if (await cache.TryGetAsync<T>(
                    key,
                    options.Duplicate()
                        .SetAllowStaleOnReadOnly()
                        .SetSkipDistributedCache(true, true),
                    cancellationToken) is { HasValue: true } entry)
            {
                cachedResults[key] = entry.Value;

                if (await cache.TryGetAsync<T>(
                        key,
                        options.Duplicate()
                            .SetAllowStaleOnReadOnly(false)
                            .SetSkipDistributedCache(true, true),
                        cancellationToken) is { HasValue: false })
                {
                    staleKeys.Add(key);
                }
            }
            else
            {
                missingKeys.Add(key);
            }
        }));

        // Using GetOrSetAsync for stampede protection, in case of multiple parallel requests with same keys.
        var missingHashKey = GetKeyHash(typeof(T), missingKeys.ToImmutableSortedSet());
        var missingResults = !missingKeys.IsEmpty ? await cache.GetOrSetAsync(
            missingHashKey,
            async (ct) =>
            {
                var results = await factory(missingKeys.ToArray(), ct);
                await Task.WhenAll(results.Select(async result =>
                {
                    var (key, value) = result;
                    await cache.SetAsync(key, value, options, ct);
                }));
                
                return results;
            }, 
            new FusionCacheEntryOptions()
                .SetDuration(TimeSpan.FromMinutes(1))
                .SetFailSafe(false),
            cancellationToken) : new Dictionary<string, T>();

        if (!staleKeys.IsEmpty)
        {
            cache.UpdateStaleAsync(staleKeys, factory, options, cancellationToken);
        }
        
        return missingResults
            .Concat(cachedResults)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// Update stale keys in the background.
    /// </summary>
    /// <param name="cache">The <see cref="IFusionCache"/> instance</param>
    /// <param name="staleKeys">A <see cref="SortedSet{T}"/> of <see cref="string"/> keys</param>
    /// <param name="factory">The batch updater function. Receives an array of <see cref="string" /> stale keys,
    /// and should return an <see cref="IDictionary{TKey,TValue}"/> of type <see cref="string"/>,<see cref="T" /></param>
    /// <param name="options">The <see cref="FusionCacheEntryOptions" /> used for setting the individual cache entries.
    /// If null is passed, the default options will be used.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <typeparam name="T">The type of the value in the cache</typeparam>
    private static void UpdateStaleAsync<T>(this IFusionCache cache,
        ConcurrentBag<string> staleKeys, Func<string[], CancellationToken, Task<IDictionary<string, T>>> factory,
        FusionCacheEntryOptions? options, CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            var staleOptions = new FusionCacheEntryOptions()
                .SetDuration(TimeSpan.FromMinutes(1))
                .SetAllowStaleOnReadOnly()
                .SetFailSafe(true);

            var staleHashKey = GetKeyHash(typeof(T), staleKeys.ToImmutableSortedSet());
            await cache.GetOrSetAsync(
                staleHashKey,
                async (ct) =>
                {
                    var results = await factory(staleKeys.ToArray(), ct);
                    await Task.WhenAll(results.Select(async result =>
                    {
                        var (key, value) = result;
                        await cache.SetAsync(key, value, options, ct);
                    }));
                
                    return results;
                },
                staleOptions, cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Build a unique hash for a set of keys.
    /// </summary>
    /// <param name="type">The type of the entity used for this cache, used as suffix</param>
    /// <param name="keys">A <see cref="SortedSet{T}"/> of <see cref="string"/> keys</param>
    /// <returns>A hash built from the collection of keys + the typename</returns>
    private static string GetKeyHash(Type type, ImmutableSortedSet<string> keys)
    {
        var hash = SHA256.HashData(
            keys.SelectMany(k => Encoding.UTF8.GetBytes(k))
                .Concat(Encoding.UTF8.GetBytes(type.FullName ?? throw new InvalidOperationException($"Invalid type \"{type}\" for cache key.")
                ))
                .ToArray());
        return Convert.ToHexStringLower(hash);
    }
}