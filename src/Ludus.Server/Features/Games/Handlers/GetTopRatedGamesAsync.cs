using System.Security.Claims;
using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;
using Marten.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public record GetTopRatedGamesResponse(
    IEnumerable<GameDto> Items,
    long TotalItemCount,
    long PageCount,
    long PageNumer,
    bool IsLastPage
) : IPaginatedResponse<GameDto>;

public class GetTopRatedGamesQuery : IPaginationParameters
{
    [FromQuery(Name = "ps")]
    public int PageSize { get; set; } = 40;

    [FromQuery(Name = "pn")]
    public int PageNumber { get; set; } = 1;
}

public static class GetTopRatedGamesAsync
{
    public static async Task<Ok<GetTopRatedGamesResponse>> Handler(
        [FromServices] IGameStore store,
        [FromServices] IGameService gameService,
        ClaimsPrincipal user,
        [AsParameters] GetTopRatedGamesQuery query
    )
    {
        await using var session = store.QuerySession();

        var games = await session
            .Query<Game>()
            .Where(x => x.GameType.Id == 0)
            .OrderByDescending(x => x.RatingCount)
            .ThenByDescending(x => x.Rating)
            .ToPagedListAsync(query.PageNumber, query.PageSize);
        var previews = await gameService.GetGameDtosAsync(user, games);
        return TypedResults.Ok(
            new GetTopRatedGamesResponse(
                previews,
                games.TotalItemCount,
                games.PageCount,
                games.PageNumber,
                games.IsLastPage
            )
        );
    }
}
