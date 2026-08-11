namespace Catalog.Features.Games.Browse.GetOverview;

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

        var gameOverview = await gameService.GetOverviewAsync(parsedGameId, cancellationToken);

        if (gameOverview is null)
            return Results.NotFound();

        return Results.Ok(new GetGameOverviewResponse(gameOverview));
    }
}
