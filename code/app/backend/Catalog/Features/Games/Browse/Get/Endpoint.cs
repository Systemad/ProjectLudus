namespace Catalog.Features.Games.Browse.Get;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<GetGameResponse>>> HandleAsync(
        string gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var game = await gameService.GetDetailsAsync(parsedGameId, cancellationToken);
        return game is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new GetGameResponse(game));
    }
}
