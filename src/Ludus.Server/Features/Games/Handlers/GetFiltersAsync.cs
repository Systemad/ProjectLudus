using Ludus.Server.Features.Games.Filters;
using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public record GetFiltersResponse(
    IEnumerable<GenreFilter> Genres,
    IEnumerable<PlatformFilter> Platforms,
    IEnumerable<GameTypeFilter> GameTypes,
    IEnumerable<ThemeFilter> Themes,
    IEnumerable<GameModeFilter> GameModes,
    IEnumerable<GameEngineFilter> GameEngines,
    IEnumerable<PlayerPerspectiveFilter> PlayerPerspectives
);

public class GetFiltersAsync
{
    public static async Task<IResult> Handler([FromServices] IGameStore store)
    {
        await using var session = store.QuerySession();
        var genres = await session
            .Query<Genre>()
            .Select(x => new GenreFilter(x.Id, x.Name))
            .ToListAsync();
        var platforms = await session
            .Query<Platform>()
            .Select(x => new PlatformFilter(x.Id, x.Name))
            .ToListAsync();
        var gameTypes = await session
            .Query<InternalGameType>()
            .Select(x => new GameTypeFilter(x.OriginalId, x.Type))
            .ToListAsync();
        var themes = await session
            .Query<Theme>()
            .Select(x => new ThemeFilter(x.Id, x.Name))
            .ToListAsync();
        var gameModes = await session
            .Query<GameMode>()
            .Select(x => new GameModeFilter(x.Id, x.Name))
            .ToListAsync();
        var gameEngines = await session
            .Query<GameEngineFilter>()
            .Select(x => new GameEngineFilter(x.Id, x.Name))
            .ToListAsync();
        var playerPerspective = await session
            .Query<PlayerPerspective>()
            .Select(x => new PlayerPerspectiveFilter(x.Id, x.Name))
            .ToListAsync();

        var filters = new GetFiltersResponse(
            genres,
            platforms,
            gameTypes,
            themes,
            gameModes,
            gameEngines,
            playerPerspective
        );
        return TypedResults.Ok(filters);
    }
}
