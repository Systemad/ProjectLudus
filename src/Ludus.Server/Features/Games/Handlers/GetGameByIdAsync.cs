using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public static class GetGameByIdAsync
{
    public static async Task<Results<Ok<Game>, NotFound>> Handler(
        [FromServices] IGameStore store,
        long id
    )
    {
        await using var session = store.QuerySession();
        var game = await session.Query<Game>().Where(g => g.Id == id).FirstOrDefaultAsync();
        if (game is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(game);
    }
}
