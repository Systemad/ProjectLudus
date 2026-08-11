using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.IGDB.GetPopscore;

public static class Endpoint
{
    public static async Task<Results<Ok<PagedGamesResponse>, ValidationProblem>> HandleAsync(
        [AsParameters] Request request,
        IIGDBService igdbService,
        CancellationToken cancellationToken
    )
    {
        if (request.From.HasValue && request.To.HasValue && request.From.Value >= request.To.Value)
        {
            return TypedResults.ValidationProblem(
                new Dictionary<string, string[]> { ["To"] = ["To must be later than From."] }
            );
        }

        var popularityTypeId = request.PopularityTypeId ?? "9";
        if (!ApiId.TryParse(popularityTypeId, out var parsedPopularityTypeId) || parsedPopularityTypeId < 1)
        {
            return TypedResults.ValidationProblem(
                new Dictionary<string, string[]> { ["PopularityTypeId"] = ["Must be a positive numeric ID."] }
            );
        }

        var result = await igdbService.GetPopscoreAsync(
            parsedPopularityTypeId,
            request.From,
            request.To,
            request.PageNumber,
            request.Size,
            cancellationToken
        );

        return TypedResults.Ok(result);
    }
}
