namespace CatalogAPI.Features.Calendar.GetGamesCalendar;

public static class GetGamesCalendarEndpoint
{
    private sealed record CalendarResponse
    {
        public required int Year { get; init; }
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
            .MapGet("/{year:int}", GetGamesCalendarAsync)
            .WithName($"{EndpointMetadata.Calendar}/GetGames")
            .WithTags(EndpointMetadata.Calendar)
            .Produces<CalendarResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> GetGamesCalendarAsync(
        AppDbContext db,
        int year,
        CancellationToken cancellationToken
    )
    {
        
        if (year is < 1 or > 9999)
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    [nameof(year)] = ["Year must be between 1 and 9999."],
                }
            );
        }
        
        var yearStart = Instant.FromUtc(year, 1, 1, 0, 0);
        var nextYearStart = yearStart.Plus(
            Duration.FromDays(DateTime.IsLeapYear(year) ? 366 : 365)
        );

        var games = await db.GamesSearches
            .Where(e => e.FirstReleaseDateUtc >= yearStart && e.FirstReleaseDateUtc < nextYearStart)
            .OrderByDescending(g => 
                // 1. Core Hype (High Weight)
                ((g.Hypes ?? 0) * 3) + 
                ((g.SteamMostWishlistedUpcoming ?? 0) * 6) +
        
                // 2. General Interest (Medium Weight)
                //((g.Follows ?? 0) * 2) + 
                (g.IgdbWantToPlay ?? 0) +
        
                // 3. Current Momentum (Low Weight)
                ((g.IgdbVisits ?? 0) * 0.1)
            )
            //.ThenBy(g => g.FirstReleaseDateUtc)
            .Take(50)
            .ToListAsync(cancellationToken);

        var chronologicalHighlights = games
            .OrderBy(g => g.FirstReleaseDateUtc)
            .ToList();
        
        return Results.Ok(new CalendarResponse
        {
            Year = year,
            Games = GameSearchMapper.MapToDto(chronologicalHighlights)
        });
    }
}
