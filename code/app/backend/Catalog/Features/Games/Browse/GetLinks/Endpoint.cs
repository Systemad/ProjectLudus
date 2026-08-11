namespace Catalog.Features.Games.Browse.GetLinks;

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

        var websites = await gameService.GetLinksAsync(parsedGameId, cancellationToken);
        return Results.Ok(new GetGameLinksResponse(websites));
    }
}
