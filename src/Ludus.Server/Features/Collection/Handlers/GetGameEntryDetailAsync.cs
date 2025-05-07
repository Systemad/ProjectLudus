using System.Security.Claims;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Collection.Handlers;

public static class GetGameEntryDetailAsync
{
    public static async Task<Results<Ok<GameCollectionDto>, NotFound<string>>> Handler(
        long gameId,
        [FromServices] IDocumentStore db,
        [FromServices] IGameStore gameStore,
        ClaimsPrincipal user
    )
    {
        var userId = Guid.Parse(user.Identity.Name);

        await using var session = db.QuerySession();
        var gameEntry = await session
            .Query<GameCollection>()
            .Where(g => g.GameId == gameId)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        if (gameEntry is null)
            return TypedResults.NotFound("GameEntry does not exist!");

        var dto = gameEntry.ToGameEntryDto();
        return TypedResults.Ok(dto);
    }
}
