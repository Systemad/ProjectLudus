using System.Security.Claims;
using Ludus.Server.Features.Games;
using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.GameEntries.Handlers;

public record GetGameEntryPreviewResponse(
    List<GameEntryPreviewDto>? Entries,
    long TotalItemCount,
    long PageCount,
    bool IsLastPage
) : IPaginatedResponse;

public class GetGameEntryPreviewQuery : IPaginationParameters
{
    [FromQuery(Name = "ps")]
    public int PageSize { get; } = 20;

    [FromQuery(Name = "pn")]
    public int PageNumber { get; } = 1;
}

public static class GetGameEntryPreviewAsync
{
    public static async Task<
        Results<Ok<GetGameEntryPreviewResponse>, UnauthorizedHttpResult>
    > Handler(
        IDocumentStore gameDb,
        IUserStore db,
        ClaimsPrincipal user,
        [AsParameters] GetGameEntryPreviewQuery query
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.QuerySession();
        await using var gameSession = gameDb.QuerySession();
        var gameEntry = await session
            .Query<GameEntry>()
            .Where(x => x.UserId == userId)
            .ToPagedListAsync(query.PageNumber, query.PageSize);
        var previews = new List<GameEntryPreviewDto>();

        foreach (var entry in gameEntry)
        {
            var game = await session
                .Query<Game>()
                .Where(x => x.Id == entry.GameId)
                .Select(g => g.ToGameDto())
                .FirstOrDefaultAsync();
            previews.Add(entry.ToGameEntryPreviewDto(game));
        }

        var response = new GetGameEntryPreviewResponse(
            previews,
            gameEntry.TotalItemCount,
            gameEntry.PageCount,
            gameEntry.IsLastPage
        );
        return TypedResults.Ok(response);
    }
}
