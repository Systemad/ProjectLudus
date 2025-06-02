using System.Security.Claims;
using Ludus.Server.Features.Collection;
using Ludus.Shared.Features.Games;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Games.Handlers;

public class GetGameByIdQuery
{
    [FromQuery(Name = "includeSimilar")]
    public bool IncludeSimilar { get; set; }
}

public class GetGameByIdResponse
{
    public Game Game { get; set; }
    public List<GameDto>? SimilarGames { get; set; }
}

public static class GetGameByIdAsync
{
    public static async Task<Results<Ok<GetGameByIdResponse>, ProblemHttpResult>> Handler(
        [FromServices] IGameStore store,
        [FromServices] IDocumentStore db,
        [FromServices] GameService gameService,
        [AsParameters] GetGameByIdQuery query,
        ClaimsPrincipal user,
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

        var response = new GetGameByIdResponse();
        if (query.IncludeSimilar)
        {
            if (game.SimilarGames is not null && game.SimilarGames.Count > 0)
            {
                var simGames = await session
                    .Query<Game>()
                    .Where(x => x.Id.IsOneOf(game.SimilarGames.Select(s => s.Id).ToList()))
                    .ToListAsync();
                var dtos = await gameService.GetGameDtosAsync(user, simGames);
                response.SimilarGames = dtos.ToList();
            }
        }

        return TypedResults.Ok(response);
    }
}
