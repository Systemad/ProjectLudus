using System.ComponentModel.DataAnnotations;
using CatalogAPI.Features.Games.Common.Projections;

namespace CatalogAPI.Features.Calendar.GetGamesCalendar;

public static class GetGamesCalendarEndpoint
{
    public sealed record GetGamesCalendarResponse
    {
        public required int Year { get; init; }
        public required List<GameDto> Games { get; init; } = [];
    }

    public static async Task<IResult> HandleAsync(
        AppDbContext db,
        [Range(1, 9999)] int year,
        CancellationToken cancellationToken
    )
    {
        var yearStart = Instant.FromUtc(year, 1, 1, 0, 0);
        var nextYearStart = yearStart.Plus(
            Duration.FromDays(DateTime.IsLeapYear(year) ? 366 : 365)
        );

        var games = await db
            .GamesSearches.Where(e =>
                e.FirstReleaseDateUtc >= yearStart && e.FirstReleaseDateUtc < nextYearStart
            )
            .OrderByDescending(g =>
                ((g.Hypes ?? 0) * 3)
                + ((g.SteamMostWishlistedUpcoming ?? 0) * 6)
                + (g.IgdbWantToPlay ?? 0)
                + ((g.IgdbVisits ?? 0) * 0.15)
            )
            .Join(db.Games, game => game.Id, game => game.Id, (_, game) => game)
            .Take(50)
            .Select(GameDtoProjection.AsGameDto)
            .ToListAsync(cancellationToken);

        return Results.Ok(
            new GetGamesCalendarResponse
            {
                Year = year,
                Games = games.OrderBy(g => g.FirstReleaseDate).ToList(),
            }
        );
    }
}

/*
        var games = await db.Games
            .Where(e => e.FirstReleaseDateUtc >= yearStart && e.FirstReleaseDateUtc < nextYearStart)
            .OrderByDescending(g =>
                ((g.Hypes ?? 0) * 3) +
                ((g.AggregatedRating ?? 0) * 2)
            )
            .Take(50)
            .Select(GameDtoProjection.AsGameDto)
            .ToListAsync(cancellationToken);
*/
