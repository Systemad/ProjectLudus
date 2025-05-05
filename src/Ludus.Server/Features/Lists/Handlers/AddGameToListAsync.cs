using System.Security.Claims;
using Ludus.Server.Features.GameEntries.Handlers;
using Ludus.Server.Features.GameEntries.Services;
using Ludus.Server.Features.Shared;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Lists.Handlers;

public record AddGameToListParameters(Guid Id, long GameId);

public static class AddGameToListAsync
{
    public static async Task<Results<Ok, BadRequest<string>>> Handle(
        IUserStore db,
        [FromServices] GameEntryService gameEntryService,
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
        var gameEntries = await session
            .Query<GameEntry>()
            .Where(x => updateList.GameEntryIds.Contains(x.Id))
            .ToListAsync();
        if (gameEntries.Any(e => e.GameId == parameters.GameId))
            return TypedResults.BadRequest("Game is already in the list");

        var query = new GameEntryQuery(
            Id: Guid.NewGuid(),
            GameId: parameters.GameId,
            Status: GameStatus.None,
            StartDate: null,
            EndDate: null,
            Rating: null,
            Notes: null
        );

        var entry = await gameEntryService.UpsertGameEntryAsync(userId, query);
        updateList.GameEntryIds.Add(entry.Id);
        session.Store(updateList);
        await session.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
