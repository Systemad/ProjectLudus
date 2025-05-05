using Marten;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ludus.Server.Features.GameEntries.Handlers;

public static class GetGameEntryDetailAsync
{
    public static async Task<Results<Ok<GameEntryDto>, NotFound<string>>> Handler(
        IUserStore db,
        IDocumentStore gameStore,
        Guid userId,
        long gameId
    )
    {
        await using var session = db.QuerySession();
        var gameEntry = await session
            .Query<GameEntry>()
            .Where(g => g.GameId == gameId)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        if (gameEntry is null)
        {
            return TypedResults.NotFound("GameEntry does not exist!");
        }
        return TypedResults.Ok(gameEntry.ToGameEntryDto());
    }
}
