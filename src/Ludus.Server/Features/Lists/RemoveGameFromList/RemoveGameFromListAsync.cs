using System.Security.Claims;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.Lists.RemoveGameFromList;

public static class RemoveGameFromListAsync
{
    public static async Task<Results<Ok, NotFound<string>>> Handle(
        IDocumentStore db,
        ClaimsPrincipal user,
        Guid listId,
        long gameId
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var list = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == listId);
        if (list is null)
            return TypedResults.NotFound("List doesn't exist!");
        var gameEntry = await session
            .Query<GameCollection>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == gameId);

        if (gameEntry is null)
            return TypedResults.NotFound("Game entry doesnt exist!");

        if (!list.GameEntryIds.Remove(gameEntry.Id))
            return TypedResults.NotFound("Game is not in the list");

        session.Store(list);
        await session.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
