using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public static class GetGameByIdAsync
{
    public static async Task<Results<Ok<GameDetail>, ProblemHttpResult>> Handler(
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
        var similarGames = new List<GameDTO>();
        if (game.SimilarGames is not null && game.SimilarGames.Count > 0)
        {
            var simGames = await session
                .Query<Game>()
                .Where(x => x.Id.IsOneOf(game.SimilarGames.Select(s => s.Id).ToList()))
                .ToListAsync();
            similarGames = simGames.Select(x => x.ToGameDto()).ToList();
        }

        return TypedResults.Ok(new GameDetail(game, similarGames));
    }
}
