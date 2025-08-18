using Marten;
using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Common.Caching;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using ZiggyCreatures.Caching.Fusion;

namespace WebAPI.Features.Common.Games;

/*
public interface IGamePreviewService
{
    Task<IEnumerable<GamePreviewDto>> HydrateGamesAsync(
        IEnumerable<IGDBGameFlat> games,
        HashSet<long> wishlistedSet,
        HashSet<long> hypedSet
    );
}
*/
/*
public class GamePreviewService : IGamePreviewService
{
    private readonly IFusionCache _cache;
    private readonly IDocumentStore _store;

    public GamePreviewService(IFusionCache cache, IDocumentStore store)
    {
        _cache = cache;
        _store = store;
    }

    public async Task<IEnumerable<GamePreviewDto>> HydrateGamesAsync(
        IEnumerable<IGDBGameFlat> games,
        HashSet<long> wishlistedSet,
        HashSet<long> hypedSet
    )
    {
        var hydratedGames = new List<GamePreviewDto>();
        await using var session = _store.LightweightSession();
        foreach (var item in games)
        {
            //var platforms = await GetPlatformsAsync(item.Platforms);
            //var platforms = await GetOrLoadBatchAsync<Platform>(item.Platforms, "platform");
            var platforms = await _cache.GetOrLoadBatchAsync<Platform>(session, item.Platforms, CacheKeys.Platform);
            hydratedGames.Add(item.ToGamePreviewDto(platforms.ToDictionary(x => x.Id, x => x), wishlistedSet.Contains(item.Id),
                hypedSet.Contains(item.Id)));
        }

        return hydratedGames;
    }
}
*/