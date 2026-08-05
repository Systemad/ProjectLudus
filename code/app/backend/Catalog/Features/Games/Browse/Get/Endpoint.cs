namespace Catalog.Features.Games.Browse.Get;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        var game = await gameService.GetDetailsAsync(gameId, cancellationToken);
        return game is null ? Results.NotFound() : Results.Ok(new GetGameResponse(game));
    }
}
