namespace Catalog.Features.Games.Browse.GetHero;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        var hero = await gameService.GetHeroAsync(gameId, cancellationToken);
        return hero is null ? Results.NotFound() : Results.Ok(new GetGameHeroResponse(hero));
    }
}
