namespace Catalog.Features.Games.Browse.GetHero;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<GetGameHeroResponse>>> HandleAsync(
        string gameId,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var hero = await gameService.GetHeroAsync(parsedGameId, cancellationToken);
        return hero is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new GetGameHeroResponse(hero));
    }
}
