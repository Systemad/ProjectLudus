namespace Catalog.Features.Games.Browse.GetLinks;

public static class Endpoint
{
    public static async Task<Results<BadRequest, Ok<GetGameLinksResponse>>> HandleAsync(
        string gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var websites = await gameService.GetLinksAsync(parsedGameId, cancellationToken);
        return TypedResults.Ok(new GetGameLinksResponse(websites));
    }
}
