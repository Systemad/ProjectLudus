using System.Security.Claims;
using Ludus.Server.Features.Collection.Services;
using Ludus.Server.Features.Lists.Services;
using Ludus.Server.Features.Shared;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.AddGameToList;

public static class AddGameToListAsync
{
    public static async Task<Results<Ok, ProblemHttpResult>> Handler(
        IDocumentStore db,
        [FromServices] IDocumentStore userStore,
        ClaimsPrincipal user,
        Guid listId,
        long gameId
    )
    {
        await using var session = userStore.LightweightSession();

        var userId = Guid.Parse(user.Identity.Name);

        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == listId);

        if (updateList is null)
            return TypedResults.Problem(
                type: "https://example.com/problems/user-list-not-found",
                title: "Game list not found",
                detail: $"No list with ID {listId} found for the current user.",
                statusCode: StatusCodes.Status404NotFound
            );

        if (updateList.Games.Contains(gameId))
            return null;

        updateList.Games.Add(gameId);
        session.Store(updateList);
        await session.SaveChangesAsync();

        return TypedResults.Ok();
    }
}
