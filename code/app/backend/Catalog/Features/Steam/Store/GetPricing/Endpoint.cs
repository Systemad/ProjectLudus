namespace Catalog.Features.Steam.Store.GetPricing;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long gameId,
        ISteamService steamService,
        CancellationToken cancellationToken
    )
    {
        var pricing = await steamService.GetPricingAsync(gameId, cancellationToken);
        return pricing is null ? Results.NotFound() : Results.Ok(pricing);
    }
}
