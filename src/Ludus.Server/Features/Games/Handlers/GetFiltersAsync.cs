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
    IEnumerable<GameEngineFilter> GameEngines
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
            .Query<GameType>()
            .Select(x => new GameTypeFilter(x.Id, x.Type))
            .ToListAsync();
        var themes = await session
            .Query<ThemeFilter>()
            .Select(x => new ThemeFilter(x.Id, x.Name))
            .ToListAsync();
        var gameModes = await session
            .Query<GameModeFilter>()
            .Select(x => new GameModeFilter(x.Id, x.Name))
            .ToListAsync();
        var gameEngines = await session
            .Query<GameEngineFilter>()
            .Select(x => new GameEngineFilter(x.Id, x.Name))
            .ToListAsync();

        var filters = new GetFiltersResponse(
            genres,
            platforms,
            gameTypes,
            themes,
            gameModes,
            gameEngines
        );
        return TypedResults.Ok(filters);
    }
}
