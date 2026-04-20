namespace CatalogAPI.Features.Games.GetByReleaseDateRange;

public static class GetByReleaseDateRangeEndpoint
{
    private record GamesReleaseDateRangeResponse(
        long From,
        long To,
        int Limit,
        List<GamesSearchDto> Games
    );

    public static IEndpointRouteBuilder MapGetByReleaseDateRangeEndpoint(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        routeBuilder
            .MapGet("/release-date-range", GetGamesByReleaseDateRangeAsync)
            .Produces<GamesReleaseDateRangeResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return routeBuilder;
    }

    private static async Task<IResult> GetGamesByReleaseDateRangeAsync(
        [AsParameters] GetByReleaseDateRangeQuery query,
        IValidator<GetByReleaseDateRangeQuery> validator,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        ValidationResult validationResult = await validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var games = await db
            .GamesSearches.Where(g => g.FirstReleaseDate.HasValue)
            .Where(g =>
                g.FirstReleaseDate!.Value >= query.From && g.FirstReleaseDate.Value <= query.To
            )
            .OrderBy(g => g.FirstReleaseDate)
            .ThenByDescending(g => g.AggregatedRating)
            .Take(query.Limit)
            .ToListAsync(cancellationToken);

        return Results.Ok(
            new GamesReleaseDateRangeResponse(
                query.From,
                query.To,
                query.Limit,
                GameSearchMapper.MapToDto(games)
            )
        );
    }
}
