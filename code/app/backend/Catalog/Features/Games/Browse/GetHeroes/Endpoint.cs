using Microsoft.AspNetCore.Mvc;

namespace Catalog.Features.Games.Browse.GetHeroes;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        [FromQuery(Name = "gameIds")] string[] gameIds,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (gameIds.Length is < 1 or > 50)
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    [nameof(gameIds)] = ["Provide between 1 and 50 game IDs."],
                }
            );
        }

        if (gameIds.Any(gameId => !ApiId.TryParse(gameId, out _)))
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    [nameof(gameIds)] = ["Game IDs must be non-negative integers."],
                }
            );
        }

        var parsedGameIds = gameIds
            .Select(gameId =>
            {
                ApiId.TryParse(gameId, out var parsedGameId);
                return parsedGameId;
            })
            .Distinct()
            .ToArray();
        var games = await gameService.GetHeroesAsync(parsedGameIds, cancellationToken);

        return Results.Ok(new GetGameHeroesResponse(games));
    }
}
