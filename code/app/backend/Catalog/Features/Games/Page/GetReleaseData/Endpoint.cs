namespace Catalog.Features.Games.Page.GetReleaseData;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        var data = await gameService.GetReleaseDataAsync(gameId, cancellationToken);
        return data is null
            ? Results.NotFound()
            : Results.Ok(new GetGamePageReleaseDataResponse(data));
    }
}
