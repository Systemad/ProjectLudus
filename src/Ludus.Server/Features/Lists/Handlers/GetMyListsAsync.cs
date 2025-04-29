using System.Security.Claims;
using Ludus.Server.Features.User.DTO;
using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.Lists.Handlers;

public record GetMyListsAsyncResult(
    Guid Id,
    string Name,
    bool Public,
    IEnumerable<Game> Games,
    int Count
);

public static class GetMyListsAsync
{
    public static async Task<
        Results<Ok<List<GetMyListsAsyncResult>>, UnauthorizedHttpResult>
    > Handle(IUserStore db, IDocumentStore gameStore, ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated ?? false)
        {
            var userId = Guid.Parse(user.Identity.Name);
            await using var session = db.QuerySession();
            await using var gameSession = gameStore.QuerySession();

            var lists = await session
                .Query<UserGameList>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
            var result = new List<GetMyListsAsyncResult>();
            foreach (var item in lists)
            {
                var previewGameIds = item.GamesIds.Take(6).ToList();

                var games = await gameSession.LoadManyAsync<Game>(previewGameIds);
                var listItem = new GetMyListsAsyncResult(
                    item.Id,
                    item.Name,
                    item.Public,
                    games,
                    games.Count
                );
                result.Add(listItem);
            }

            return TypedResults.Ok(result);
        }

        return TypedResults.Unauthorized();
    }
}
