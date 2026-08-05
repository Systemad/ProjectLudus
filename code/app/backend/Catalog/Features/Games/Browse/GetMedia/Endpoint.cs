namespace Catalog.Features.Games.Browse.GetMedia;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        var media = await gameService.GetMediaAsync(gameId, cancellationToken);
        return media is null ? Results.NotFound() : Results.Ok(new GetGameMediaResponse(media));
    }
}
