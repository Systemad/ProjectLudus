namespace Catalog.Features.Games.Page.GetReleaseData;

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

        var data = await gameService.GetReleaseDataAsync(parsedGameId, cancellationToken);
        return data is null
            ? Results.NotFound()
            : Results.Ok(new GetGamePageReleaseDataResponse(data));
    }
}
