using CatalogAPI.Features.Games.Common.Projections;
using NodaTime.Extensions;

namespace CatalogAPI.Features.Games.GetByReleaseDateRange;

public static class GetByReleaseDateRangeEndpoint
{
    public record GetByReleaseDateRangeResponse(
        DateOnly Start,
        DateOnly End,
        int Limit,
        List<GameDto> Games
    );

    public static async Task<IResult> HandleAsync(
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
            .Games.Where(g => g.FirstReleaseDateUtc >= startUtc && g.FirstReleaseDateUtc <= endUtc)
            .OrderBy(g => g.FirstReleaseDateUtc)
            .ThenByDescending(g => g.AggregatedRating)
            .Take(query.Limit)
            .Select(GameDtoProjection.AsGameDto)
            .ToListAsync(cancellationToken);

        return Results.Ok(
            new GetByReleaseDateRangeResponse(query.Start, query.End, query.Limit, games)
        );
    }
}
