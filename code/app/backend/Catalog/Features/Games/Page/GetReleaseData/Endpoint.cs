namespace Catalog.Features.Games.Page.GetReleaseData;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<GetGamePageReleaseDataResponse>>> HandleAsync(
        string gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var data = await gameService.GetReleaseDataAsync(parsedGameId, cancellationToken);
        return data is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new GetGamePageReleaseDataResponse(data));
    }
}
