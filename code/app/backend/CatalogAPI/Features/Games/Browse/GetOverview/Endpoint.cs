
namespace CatalogAPI.Features.Games.Browse.GetOverview;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        var gameOverview = await gameService.GetOverviewAsync(gameId, cancellationToken);

        if (gameOverview is null)
            return Results.NotFound();

        return Results.Ok(new GetGameOverviewResponse(gameOverview));
    }
}
