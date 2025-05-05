using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;
using Marten;
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
    public int PageSize { get; } = 20;
    public int PageNumber { get; } = 1;
}

public static class GetTopRatedGamesAsync
{
    public static async Task<Ok<GetTopRatedGamesResponse>> Handler(
        [FromServices] IQuerySession session,
        [AsParameters] GetTopRatedGamesQuery query
    )
    {
        var games = await session
            .Query<Game>()
            .Select(x => x.ToGameDto())
            .ToPagedListAsync(query.PageNumber, query.PageSize);

        return TypedResults.Ok(
            new GetTopRatedGamesResponse(
                games,
                games.TotalItemCount,
                games.PageCount,
                games.IsLastPage
            )
        );
    }
}
