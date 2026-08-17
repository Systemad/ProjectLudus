namespace Catalog.Features.Games.Browse.GetOverview;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<GetGameOverviewResponse>>> HandleAsync(
        string gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var gameOverview = await gameService.GetOverviewAsync(parsedGameId, cancellationToken);

        if (gameOverview is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(new GetGameOverviewResponse(gameOverview));
    }
}
