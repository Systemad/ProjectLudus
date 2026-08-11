namespace Catalog.Features.Games.Browse.GetHero;

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

        var hero = await gameService.GetHeroAsync(parsedGameId, cancellationToken);
        return hero is null ? Results.NotFound() : Results.Ok(new GetGameHeroResponse(hero));
    }
}
