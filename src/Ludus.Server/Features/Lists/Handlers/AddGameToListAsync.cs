using System.Security.Claims;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.Handlers;

public record AddGameToListParameters(Guid Id, long GameId);

public static class AddGameToListAsync
{
    public static async Task<Results<Ok, BadRequest<string>>> Handle(
        IUserStore db,
        ClaimsPrincipal user,
        Guid listId,
        [FromBody] AddGameToListParameters parameters
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == listId);
        if (updateList.GamesIds.Contains(parameters.GameId))
            return TypedResults.BadRequest("Game is already in the list");

        updateList.GamesIds.Add(parameters.GameId);
        session.Store(updateList);
        await session.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
