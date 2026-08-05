namespace Catalog.Features.Games.Browse.GetSimilarGames;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        var games = await gameService.GetSimilarGamesAsync(gameId, cancellationToken);
        return Results.Ok(new GetSimilarGamesResponse(games));
    }
}
