namespace Catalog.Features.Games.Browse.GetSimilarGames;

public static class Endpoint
{
    public static async Task<Results<BadRequest, Ok<GetSimilarGamesResponse>>> HandleAsync(
        string gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var games = await gameService.GetSimilarGamesAsync(parsedGameId, cancellationToken);
        return TypedResults.Ok(new GetSimilarGamesResponse(games));
    }
}
