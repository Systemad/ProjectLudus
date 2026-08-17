namespace Catalog.Features.Steam.Store.GetReviews;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<GetReviewsResponse>>> HandleAsync(
        string gameId,
        ISteamService steamService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var reviews = await steamService.GetReviewsAsync(parsedGameId, cancellationToken);
        return reviews is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(reviews);
    }
}
