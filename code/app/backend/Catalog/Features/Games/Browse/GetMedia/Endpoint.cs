namespace Catalog.Features.Games.Browse.GetMedia;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<GetGameMediaResponse>>> HandleAsync(
        string gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var media = await gameService.GetMediaAsync(parsedGameId, cancellationToken);
        return media is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new GetGameMediaResponse(media));
    }
}
