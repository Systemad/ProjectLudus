namespace Catalog.Features.Steam.Store.GetPricing;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        string gameId,
        ISteamService steamService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return Results.BadRequest();

        var pricing = await steamService.GetPricingAsync(parsedGameId, cancellationToken);
        return pricing is null ? Results.NotFound() : Results.Ok(pricing);
    }
}
