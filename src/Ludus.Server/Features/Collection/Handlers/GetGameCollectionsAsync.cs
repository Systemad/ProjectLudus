using System.Security.Claims;
using Ludus.Server.Features.Games;
using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Collection.Handlers;

public record GetGameCollectionsResponse(
    IEnumerable<GameDto> Items,
    long TotalItemCount,
    long PageCount,
    long PageNumer,
    bool IsLastPage
) : IPaginatedResponse<GameDto>;

public class GetGameCollectionsQuery : IPaginationParameters
{
    [FromQuery(Name = "ps")]
    public int PageSize { get; } = 40;

    [FromQuery(Name = "pn")]
    public int PageNumber { get; } = 1;
}

public static class GetGameCollectionsAsync
{
    public static async Task<
        Results<Ok<GetGameCollectionsResponse>, UnauthorizedHttpResult>
    > Handler(
        [FromServices] GameService gameService,
        IGameStore gameDb,
        IDocumentStore db,
        ClaimsPrincipal user,
        [AsParameters] GetGameCollectionsQuery query
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.QuerySession();
        await using var gameSession = gameDb.QuerySession();
        var gameCollection = await session
            .Query<UserGameData>()
            .Where(x => x.UserId == userId)
            .ToPagedListAsync(query.PageNumber, query.PageSize);
        var games = await gameSession.LoadManyAsync<Game>(gameCollection.Select(x => x.GameId));

        var previews = await gameService.GetGameDtosAsync(user, games);

        var response = new GetGameCollectionsResponse(
            previews,
            gameCollection.TotalItemCount,
            gameCollection.PageCount,
            gameCollection.PageNumber,
            gameCollection.IsLastPage
        );
        return TypedResults.Ok(response);
    }
}
