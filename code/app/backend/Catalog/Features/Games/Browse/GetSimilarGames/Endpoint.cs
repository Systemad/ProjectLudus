namespace Catalog.Features.Games.Browse.GetSimilarGames;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        string gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return Results.BadRequest();

        var games = await gameService.GetSimilarGamesAsync(parsedGameId, cancellationToken);
        return Results.Ok(new GetSimilarGamesResponse(games));
    }
}
