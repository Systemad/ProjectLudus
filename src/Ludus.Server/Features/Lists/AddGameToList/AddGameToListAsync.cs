using System.Security.Claims;
using Ludus.Server.Features.Collection.Services;
using Ludus.Server.Features.Shared;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.AddGameToList;

public static class AddGameToListAsync
{
    public static async Task<Results<Ok, BadRequest<string>>> Handle(
        IDocumentStore db,
        [FromServices] GameCollectionService gameCollectionService,
        ClaimsPrincipal user,
        Guid listId,
        long gameId
    )
    {
        var userId = Guid.Parse(user.Identity.Name);
        await using var session = db.LightweightSession();
        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == listId);

        if (updateList is null)
            return TypedResults.BadRequest("List doesn't exist!");
        var gameEntry = await session
            .Query<GameCollection>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == gameId);

        if (gameEntry is null)
        {
            gameEntry = new GameCollection
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                GameId = gameId,
                Status = GameStatus.None,
            };
            session.Store(gameEntry);
        }

        if (updateList.GameEntryIds.Contains(gameEntry.Id))
            return TypedResults.BadRequest("Game is already in the list");

        updateList.GameEntryIds.Add(gameEntry.Id);
        session.Store(updateList);
        await session.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
