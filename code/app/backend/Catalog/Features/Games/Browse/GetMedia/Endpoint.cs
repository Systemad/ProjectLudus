namespace Catalog.Features.Games.Browse.GetMedia;

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

        var media = await gameService.GetMediaAsync(parsedGameId, cancellationToken);
        return media is null ? Results.NotFound() : Results.Ok(new GetGameMediaResponse(media));
    }
}
