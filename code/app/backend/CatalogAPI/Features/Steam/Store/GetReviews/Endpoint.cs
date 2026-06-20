

namespace CatalogAPI.Features.Steam.Store.GetReviews;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long gameId,
        ISteamService steamService,
        CancellationToken cancellationToken
    )
    {
        var reviews = await steamService.GetReviewsAsync(gameId, cancellationToken);
        return reviews is null ? Results.NotFound() : Results.Ok(reviews);
    }
}
