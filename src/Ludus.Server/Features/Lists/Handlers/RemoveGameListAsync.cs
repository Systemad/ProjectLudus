using System.Security.Claims;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.Handlers;

public record RemoveGameToListParameters(Guid Id, long GameId);

public static class RemoveGameListAsync
{
    public static async Task<Results<Ok, NotFound<string>>> Handle(
        IUserStore db,
        ClaimsPrincipal user,
        Guid listId,
        [FromBody] RemoveGameToListParameters parameters
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == listId);
        var removed = updateList.GamesIds.Remove(parameters.GameId);
        if (!removed)
            return TypedResults.NotFound("Game is not in the list");
        session.Store(updateList);
        await session.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
