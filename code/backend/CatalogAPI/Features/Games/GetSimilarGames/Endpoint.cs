namespace CatalogAPI.Features.Games.GetSimilarGames;

public static class Endpoint
{
    private record GetSimilarGamesResponse(List<GamesSearchDto> Games);

    public static RouteHandlerBuilder MapGetSimilarGamesEndpoint(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        return routeBuilder
            .MapGet("/{gameId:long}/similar-games", GetSimilarGamesAsync)
            .WithName($"{EndpointMetadata.Games}/GetSimilar")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetSimilarGamesResponse>(StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetSimilarGamesAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var similarGames = await db
            .Games.Where(g => g.Id == gameId)
            .SelectMany(g => g.SimilarGames)
            .Join(
                db.GamesSearches,
                similar => similar.Id,
                search => search.Id,
                (_, search) => search
            )
            .ToListAsync(cancellationToken);

        return Results.Ok(new GetSimilarGamesResponse(GameSearchMapper.MapToDto(similarGames)));
    }
}
