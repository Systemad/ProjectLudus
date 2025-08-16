using Marten;
using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Common.Caching;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using ZiggyCreatures.Caching.Fusion;

namespace WebAPI.Features.Common.Games;

public interface IGameService
{
    Task<IEnumerable<GameDto>> HydrateGamesAsync(
        IEnumerable<InsertIgdbGame> games,
        HashSet<long> wishlistedSet,
        HashSet<long> hypedSet
    );

    Task<IgdbGame> HydrateGameDetailAsync(InsertIgdbGame game);
}

public class GameService : IGameService
{
    private readonly IFusionCache _cache;
    private readonly IDocumentStore _store;

    public GameService(IFusionCache cache, IDocumentStore store)
    {
        _cache = cache;
        _store = store;
    }

    public async Task<IEnumerable<GameDto>> HydrateGamesAsync(
        IEnumerable<InsertIgdbGame> games,
        HashSet<long> wishlistedSet,
        HashSet<long> hypedSet
    )
    {
        var hydratedGames = new List<GameDto>();
        await using var session = _store.LightweightSession();
        foreach (var item in games)
        {
            //var platforms = await GetPlatformsAsync(item.Platforms);
            //var platforms = await GetOrLoadBatchAsync<Platform>(item.Platforms, "platform");
            var platforms = await _cache.GetOrLoadBatchAsync<Platform>(session, item.Platforms, CacheKeys.Platform);
            hydratedGames.Add(item.MapToGameDto(platforms, wishlistedSet.Contains(item.Id),
                hypedSet.Contains(item.Id)));
        }

        return hydratedGames;
    }

    public async Task<IgdbGame> HydrateGameDetailAsync(InsertIgdbGame game)
    {
        await using var session = _store.LightweightSession();
        var involvedCompanies = _cache.GetOrLoadBatchAsync<InvolvedCompany>(session,
            game.InvolvedCompanies.Select(x => x.Id), CacheKeys.Company);
        var gameEngines = _cache.GetOrLoadBatchAsync<GameEngine>(session,
            game.GameEngines, CacheKeys.GameEngine);
        var gameModes = _cache.GetOrLoadBatchAsync<GameMode>(session,
            game.GameModes, CacheKeys.GameMode);
        var genres = _cache.GetOrLoadBatchAsync<Genre>(session,
            game.Genres, CacheKeys.Genre);
        var keywords = _cache.GetOrLoadBatchAsync<Keyword>(session,
            game.Keywords, CacheKeys.Keyword);
        var platforms = _cache.GetOrLoadBatchAsync<Platform>(session,
            game.Platforms, CacheKeys.Platform);
        var playerPerspectives = _cache.GetOrLoadBatchAsync<PlayerPerspective>(session,
            game.PlayerPerspectives, CacheKeys.PlayerPerspective);
        var themes = _cache.GetOrLoadBatchAsync<Theme>(session,
            game.Themes, CacheKeys.Theme);

        await Task.WhenAll(
            involvedCompanies, gameEngines, gameModes,
            genres, keywords, platforms,
            playerPerspectives, themes);

        return game.MapToGame(
            await gameEngines,
            await gameModes,
            await genres,
            await involvedCompanies,
            await keywords,
            await platforms,
            await playerPerspectives,
            await themes);
    }
}

/*

    private async Task<List<T>> GetOrLoadBatchAsync<T>(IEnumerable<long> ids, string keyPrefix) where T : class, IIdentifiable
    {
        var keys = ids.Select(id => $"{keyPrefix}:{id}").ToArray();

        var result = await _cache.GetOrSetBatchAsync<T>(
            keys,
            async (batchKeys, ct) =>
            {
                // Extract IDs from keys
                var extractedIds = batchKeys.Select(k => long.Parse(k.Split(':')[1])).ToList();

                // Query DB once for all platforms
                await using var session = _store.LightweightSession();
                var platforms = await session.Query<T>()
                    .Where(p =>  extractedIds.Contains(p.Id))
                    .ToListAsync(ct);

                // Map to dictionary keyed by "platform:{id}"
                return platforms.ToDictionary(p => $"{keyPrefix}:{p.Id}", p => p);
            },
            entry =>
            {
                // Cache options for batch cache entries (e.g. expire in 1 hour)
                entry.SetDuration(TimeSpan.FromHours(1));
            });

        // Return values only, ignoring keys
        return result.Values.ToList();
    }

    private async Task<List<Platform>> GetPlatformsAsync(IEnumerable<long> ids)
    {
        var keys = ids.Select(id => $"platform:{id}").ToArray();

        var result = await _cache.GetOrSetBatchAsync<Platform>(
            keys,
            async (batchKeys, ct) =>
            {
                // Extract IDs from keys
                var platformIds = batchKeys.Select(k => long.Parse(k.Split(':')[1])).ToList();

                // Query DB once for all platforms
                await using var session = _store.LightweightSession();
                var platforms = await session.Query<Platform>()
                    .Where(p =>  platformIds.Contains(p.Id))
                    .ToListAsync(ct);

                // Map to dictionary keyed by "platform:{id}"
                return platforms.ToDictionary(p => $"platform:{p.Id}", p => p);
            },
            entry =>
            {
                // Cache options for batch cache entries (e.g. expire in 1 hour)
                entry.SetDuration(TimeSpan.FromHours(1));
            });

        // Return values only, ignoring keys
        return result.Values.ToList();
    }

    */