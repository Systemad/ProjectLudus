using System.Security.Claims;
using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.Lists.Handlers;

public record GetGamesOfUserListResult(
    Guid Id,
    string Name,
    bool Public,
    IEnumerable<Game> Games,
    long Count
);

public static class GetListGamesAsync
{
    public static async Task<Results<Ok<GetGamesOfUserListResult>, BadRequest<string>>> Handle(
        IDocumentStore gameStore,
        IUserStore db,
        ClaimsPrincipal user,
        Guid listId
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var userSession = db.QuerySession();
        await using var gameSession = gameStore.QuerySession();
        var list = await userSession
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == listId);

        if (list is null || list.GamesIds.Count <= 0)
        {
            return TypedResults.BadRequest("List is empty!");
        }

        var games = await gameSession.LoadManyAsync<Game>(list.GamesIds);

        var response = new GetGamesOfUserListResult(
            list.Id,
            list.Name,
            list.Public,
            games.ToList(),
            games.Count
        );
        return TypedResults.Ok(response);
    }
}
