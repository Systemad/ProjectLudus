using System.Security.Claims;
using Ludus.Server.Features.Games;
using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Collection.Handlers;

public record GetGameCollectionPreviewsResponse(
    List<GameCollectionPreviewDto>? Entries,
    long TotalItemCount,
    long PageCount,
    bool IsLastPage
) : IPaginatedResponse;

public class GetGameCollectionPreviewsQuery : IPaginationParameters
{
    [FromQuery(Name = "ps")]
    public int PageSize { get; } = 20;

    [FromQuery(Name = "pn")]
    public int PageNumber { get; } = 1;
}

public static class GetGameCollectionPreviewsAsync
{
    public static async Task<
        Results<Ok<GetGameCollectionPreviewsResponse>, UnauthorizedHttpResult>
    > Handler(
        IGameStore gameDb,
        IDocumentStore db,
        ClaimsPrincipal user,
        [AsParameters] GetGameCollectionPreviewsQuery query
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.QuerySession();
        await using var gameSession = gameDb.QuerySession();
        var gameEntry = await session
            .Query<GameCollection>()
            .Where(x => x.UserId == userId)
            .ToPagedListAsync(query.PageNumber, query.PageSize);
        var previews = new List<GameCollectionPreviewDto>();

        foreach (var entry in gameEntry)
        {
            var game = await session
                .Query<Game>()
                .Where(x => x.Id == entry.GameId)
                .FirstOrDefaultAsync();

            previews.Add(entry.ToGameEntryPreviewDto(game.ToGameDto()));
        }

        var response = new GetGameCollectionPreviewsResponse(
            previews,
            gameEntry.TotalItemCount,
            gameEntry.PageCount,
            gameEntry.IsLastPage
        );
        return TypedResults.Ok(response);
    }
}
