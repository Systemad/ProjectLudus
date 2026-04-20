using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Features.Calendar.GetGamesCalendar;

public static class GetGamesCalendarEndpoint
{
    private sealed record CalendarResponse
    {
        public required DateTime WeekStart { get; init; }
        public required DateTime WeekEnd { get; init; }
        public required List<GamesSearchDto> Games { get; init; } = [];
    }

    private static DateTime GetStartOfWeek(DateTime date)
    {
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.Date.AddDays(-diff);
    }
    
    public static RouteHandlerBuilder MapGetGamesCalendarEndpoint(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        return routeBuilder
            .MapGet("/{startDate}", GetGamesCalendarAsync)
            .WithName($"{EndpointMetadata.Calendar}/GetGames")
            .WithTags(EndpointMetadata.Calendar)
            .Produces<CalendarResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> GetGamesCalendarAsync(
        AppDbContext db,
        CancellationToken cancellationToken,
        DateTimeOffset startDate
    )
    {
        var weekStart = GetStartOfWeek(startDate.UtcDateTime);
        var weekEnd = weekStart.AddDays(7);

        var startInstant = Instant.FromDateTimeUtc(weekStart);
        var endInstant = Instant.FromDateTimeUtc(weekEnd);

        var games = await db.GamesSearches
            .Where(g =>
                g.FirstReleaseDateUtc >= startInstant &&
                g.FirstReleaseDateUtc < endInstant
            )
            .OrderBy(g => g.FirstReleaseDateUtc)
            .ThenByDescending(g => g.AggregatedRating)
            .ToListAsync(cancellationToken);

        return Results.Ok(new CalendarResponse
        {
            WeekStart = weekStart,
            WeekEnd = weekEnd,
            Games = GameSearchMapper.MapToDto(games)
        });
    }
}
