using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public static class GetGameByIdAsync
{
    public static async Task<Results<Ok<Game>, ProblemHttpResult>> Handler(
        [FromServices] IGameStore store,
        long id
    )
    {
        await using var session = store.QuerySession();
        var game = await session.Query<Game>().Where(g => g.Id == id).FirstOrDefaultAsync();
        if (game is null)
        {
            return TypedResults.Problem(
                type: "Not found",
                title: "Game not found",
                detail: "Game is not found by its ID",
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        return TypedResults.Ok(game);
    }
}
