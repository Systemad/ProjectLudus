namespace Catalog.Features.Games.Browse.GetLinks;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        var websites = await gameService.GetLinksAsync(gameId, cancellationToken);
        return Results.Ok(new GetGameLinksResponse(websites));
    }
}
