using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;
using Marten.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public record GetTopRatedGamesResponse(
    IEnumerable<GameDTO> Games,
    long TotalItemCount,
    long PageCount,
    bool IsLastPage
) : IPaginatedResponse;

public class GetTopRatedGamesQuery : IPaginationParameters
{
    [FromQuery(Name = "ps")]
    public int PageSize { get; set; } = 20;

    [FromQuery(Name = "pn")]
    public int PageNumber { get; set; } = 1;
}

public static class GetTopRatedGamesAsync
{
    public static async Task<Ok<GetTopRatedGamesResponse>> Handler(
        [FromServices] IGameStore store,
        [AsParameters] GetTopRatedGamesQuery query
    )
    {
        await using var session = store.QuerySession();
        var games = await session.Query<Game>().ToPagedListAsync(query.PageNumber, query.PageSize);
        var mappedGames = games.Select(x => x.ToGameDto());

        return TypedResults.Ok(
            new GetTopRatedGamesResponse(
                mappedGames,
                games.TotalItemCount,
                games.PageCount,
                games.IsLastPage
            )
        );
    }
}
