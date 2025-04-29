using Ludus.Shared.Features.Games;
using Marten;
using Marten.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public class TopRatedGamesParameters
{
    [FromQuery(Name = "p")]
    public int? PageSize { get; set; } = 20;

    [FromQuery(Name = "pn")]
    public int? PageNumber { get; set; } = 1;
}

public record GetTopRatedGamesResult(IEnumerable<Game> Games, long TotalItems, long TotalPages);

public static class GetTopRatedGamesAsync
{
    public static async Task<Ok<GetTopRatedGamesResult>> Handler(
        [FromServices] IQuerySession session,
        [AsParameters] TopRatedGamesParameters parameters
    )
    {
        var games = await session
            .Query<Game>()
            .ToPagedListAsync(parameters.PageNumber ?? 1, parameters.PageSize ?? 20);

        return TypedResults.Ok(
            new GetTopRatedGamesResult(games, games.TotalItemCount, games.PageCount)
        );
    }
}
