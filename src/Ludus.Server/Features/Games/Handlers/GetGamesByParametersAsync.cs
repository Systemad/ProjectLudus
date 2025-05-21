using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Linq;
using Marten.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public class GameSearchQuery : IPaginationParameters
{
    [FromQuery(Name = "ps")]
    public int PageSize { get; set; } = 40;

    [FromQuery(Name = "pn")]
    public int PageNumber { get; set; } = 1;

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

    [FromQuery(Name = "gametypeid")]
    public long[]? GameTypeId { get; set; } = null;

    [FromQuery(Name = "ppsid")]
    public long[]? PlayerPerspectiveId { get; set; } = null;
}

public record GetSearchGamesResult(
    IEnumerable<GameDTO> Games,
    long TotalItemCount,
    long PageCount,
    long PageNumer,
    bool IsLastPage
) : IPaginatedResponse;

public static class GetGamesByParametersAsync
{
    public static async Task<Ok<GetSearchGamesResult>> Handler(
        [FromServices] IGameStore store,
        [AsParameters] GameSearchQuery query
    )
    {
        await using var session = store.QuerySession();
        var gameQuery = session.Query<Game>();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x =>
                        x.Name.Contains(query.Name, StringComparison.CurrentCultureIgnoreCase)
                    );
        }

        if (query.GenreId is not null && query.GenreId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.Genres.Any(g => query.GenreId.Contains(g.Id)));
        }
        if (query.GameTypeId is not null && query.GameTypeId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => query.GameTypeId.Contains(x.GameType.Id));
        }
        if (query.PlatformId is not null && query.PlatformId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.Platforms.Any(g => query.PlatformId.Contains(g.Id)));
        }

        if (query.GameModeId is not null && query.GameModeId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.GameModes.Any(g => query.GameModeId.Contains(g.Id)));
        }

        if (query.ThemeId is not null && query.ThemeId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.Themes.Any(g => query.ThemeId.Contains(g.Id)));
        }

        if (query.PlayerPerspectiveId is not null && query.PlayerPerspectiveId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x =>
                        x.PlayerPerspectives.Any(g => query.PlayerPerspectiveId.Contains(g.Id))
                    );
        }

        var games = await gameQuery.ToPagedListAsync(query.PageNumber, query.PageSize);
        var mappedGames = games.Select(x => x.ToGameDto());
        return TypedResults.Ok(
            new GetSearchGamesResult(
                mappedGames,
                games.TotalItemCount,
                games.PageCount,
                games.PageNumber,
                games.IsLastPage
            )
        );
    }
}
