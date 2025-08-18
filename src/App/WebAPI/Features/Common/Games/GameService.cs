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
    Task<IgdbGameDto> HydrateGameDetailAsync(IGDBGameFlat igdbGameFlat);
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



    public async Task<IgdbGameDto> HydrateGameDetailAsync(IGDBGameFlat igdbGameFlat)
    {
        await using var session = _store.LightweightSession();
        var involvedCompanies = _cache.GetOrLoadBatchAsync<InvolvedCompany>(session,
            igdbGameFlat.InvolvedCompanies.Select(x => x.Id), CacheKeys.Company);
        var gameEngines = _cache.GetOrLoadBatchAsync<GameEngine>(session,
            igdbGameFlat.GameEngines, CacheKeys.GameEngine);
        var gameModes = _cache.GetOrLoadBatchAsync<GameMode>(session,
            igdbGameFlat.GameModes, CacheKeys.GameMode);
        var genres = _cache.GetOrLoadBatchAsync<Genre>(session,
            igdbGameFlat.Genres, CacheKeys.Genre);
        var keywords = _cache.GetOrLoadBatchAsync<Keyword>(session,
            igdbGameFlat.Keywords, CacheKeys.Keyword);
        var platforms = _cache.GetOrLoadBatchAsync<Platform>(session,
            igdbGameFlat.Platforms, CacheKeys.Platform);
        var playerPerspectives = _cache.GetOrLoadBatchAsync<PlayerPerspective>(session,
            igdbGameFlat.PlayerPerspectives, CacheKeys.PlayerPerspective);
        var themes = _cache.GetOrLoadBatchAsync<Theme>(session,
            igdbGameFlat.Themes, CacheKeys.Theme);

        await Task.WhenAll(
            involvedCompanies, gameEngines, gameModes,
            genres, keywords, platforms,
            playerPerspectives, themes);

        return igdbGameFlat.ToGameDto(
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
