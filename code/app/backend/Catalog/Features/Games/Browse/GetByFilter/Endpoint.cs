using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.Games.Browse.GetByFilter;

public static class Endpoint
{
    public static async Task<Results<Ok<PagedGamesResponse>, ValidationProblem>> HandleAsync(
        [AsParameters] Request request,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        if (request.From.HasValue && request.To.HasValue && request.From.Value >= request.To.Value)
        {
            return TypedResults.ValidationProblem(
                new Dictionary<string, string[]> { ["To"] = ["To must be later than From."] }
            );
        }

        var result = await gameService.GetGamesByFilterAsync(request, cancellationToken);

        return TypedResults.Ok(result);
    }
}
