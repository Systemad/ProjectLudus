using NodaTime.Extensions;

namespace CatalogAPI.Features.Games.GetByReleaseDateRange;

public static class Endpoint
{
    private record GamesReleaseDateRangeResponse(
        DateOnly Start,
        DateOnly End,
        int Limit,
        List<GamesSearchDto> Games
    );

    public static RouteHandlerBuilder MapGetGamesByReleaseDateRangeEndpoint(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        return routeBuilder
            .MapGet("/release-date-range", GetGamesByReleaseDateRangeAsync)
            .WithName($"{EndpointMetadata.Games}/GetReleaseDateRange")
            .WithTags(EndpointMetadata.Games)
            .Produces<GamesReleaseDateRangeResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
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

        var startUtc = new DateTimeOffset(
            query.Start.ToDateTime(TimeOnly.MinValue),
            TimeSpan.Zero
        ).ToInstant();
        var endUtc = new DateTimeOffset(
            query.End.ToDateTime(TimeOnly.MaxValue),
            TimeSpan.Zero
        ).ToInstant();

        var games = await db
            .GamesSearches.Where(g =>
                g.FirstReleaseDateUtc >= startUtc && g.FirstReleaseDateUtc <= endUtc
            )
            .OrderBy(g => g.FirstReleaseDateUtc)
            .ThenByDescending(g => g.AggregatedRating)
            .Take(query.Limit)
            .ToListAsync(cancellationToken);

        return Results.Ok(
            new GamesReleaseDateRangeResponse(
                query.Start,
                query.End,
                query.Limit,
                GameSearchMapper.MapToDto(games)
            )
        );
    }
}
