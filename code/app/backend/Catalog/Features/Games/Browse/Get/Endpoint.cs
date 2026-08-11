namespace Catalog.Features.Games.Browse.Get;

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

        var game = await gameService.GetDetailsAsync(parsedGameId, cancellationToken);
        return game is null ? Results.NotFound() : Results.Ok(new GetGameResponse(game));
    }
}
