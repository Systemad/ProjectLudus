namespace CatalogAPI.Features.Steam.Store.GetPricing;

public sealed record GetPricingResponse(
    long GameId, long? SteamAppId, int? FinalCents, int? DiscountPercent,
    string? Currency, int? High30d, int? Low30d);
