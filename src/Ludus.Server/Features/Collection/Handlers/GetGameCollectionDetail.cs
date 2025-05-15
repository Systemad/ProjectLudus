using System.Security.Claims;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Collection.Handlers;

public static class GetGameCollectionDetailAsync
{
    public static async Task<Results<Ok<GameCollectionDto>, ProblemHttpResult>> Handler(
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
        {
            return TypedResults.Problem(
                type: "Not found",
                title: "Game not found",
                detail: "Game is not found by its ID",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var dto = gameEntry.ToGameEntryDto();
        return TypedResults.Ok(dto);
    }
}
