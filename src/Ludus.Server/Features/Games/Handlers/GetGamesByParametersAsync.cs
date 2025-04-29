using Ludus.Shared.Features.Games;
using Marten;
using Marten.Linq;
using Marten.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public class GameSearchParameters
{
    [FromQuery(Name = "ps")]
    public int? PageSize { get; set; } = 20;

    [FromQuery(Name = "pn")]
    public int? PageNumber { get; set; } = 1;

    [FromQuery(Name = "name")]
    public string? Name { get; set; } = null;

    [FromQuery(Name = "genreid")]
    public long[]? GenreId { get; set; } = null;

    [FromQuery(Name = "platformid")]
    public long[]? PlatformId { get; set; } = null;

    [FromQuery(Name = "gamemodeid")]
    public long[]? GameModeId { get; set; } = null;

    [FromQuery(Name = "themeid")]
    public long[]? ThemeId { get; set; } = null;

    [FromQuery(Name = "ppsid")]
    public long[]? PlayerPerspectiveId { get; set; } = null;
}

public record GetSearchGamesResult(IEnumerable<Game> Games, long TotalItems, long TotalPages);

public static class GetGamesByParametersAsync
{
    public static async Task<Ok<GetSearchGamesResult>> Handler(
        [FromServices] IQuerySession session,
        [AsParameters] GameSearchParameters parameters
    )
    {
        var gameQuery = session.Query<Game>();

        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            gameQuery =
                (IMartenQueryable<Game>)gameQuery.Where(x => x.Name.Contains(parameters.Name));
        }

        if (parameters.GenreId is not null && parameters.GenreId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.Genres.Any(g => parameters.GenreId.Contains(g.Id)));
        }

        if (parameters.PlatformId is not null && parameters.PlatformId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x =>
                        x.Platforms.Any(g => parameters.PlatformId.Contains(g.Id))
                    );
        }

        if (parameters.GameModeId is not null && parameters.GameModeId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x =>
                        x.GameModes.Any(g => parameters.GameModeId.Contains(g.Id))
                    );
        }

        if (parameters.ThemeId is not null && parameters.ThemeId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.Themes.Any(g => parameters.ThemeId.Contains(g.Id)));
        }

        if (parameters.PlayerPerspectiveId is not null && parameters.PlayerPerspectiveId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x =>
                        x.PlayerPerspectives.Any(g => parameters.PlayerPerspectiveId.Contains(g.Id))
                    );
        }

        var games = await gameQuery.ToPagedListAsync(
            parameters.PageNumber ?? 1,
            parameters.PageSize ?? 20
        );
        return TypedResults.Ok(
            new GetSearchGamesResult(games, games.TotalItemCount, games.PageCount)
        );
    }
}
