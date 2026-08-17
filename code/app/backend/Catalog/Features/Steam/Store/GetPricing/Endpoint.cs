namespace Catalog.Features.Steam.Store.GetPricing;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<GetPricingResponse>>> HandleAsync(
        string gameId,
        ISteamService steamService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var pricing = await steamService.GetPricingAsync(parsedGameId, cancellationToken);
        return pricing is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(pricing);
    }
}
