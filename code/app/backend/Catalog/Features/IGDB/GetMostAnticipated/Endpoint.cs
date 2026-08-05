using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.IGDB.GetMostAnticipated;

public static class Endpoint
{
    public static async Task<Ok<PagedGamesResponse>> HandleAsync(
        [AsParameters] PageRequest request,
        IIGDBService igdbService,
        CancellationToken cancellationToken
    )
    {
        var games = await igdbService.GetMostAnticipatedAsync(
            request.PageNumber,
            request.Size,
            cancellationToken
        );

        return TypedResults.Ok(games);
    }
}
