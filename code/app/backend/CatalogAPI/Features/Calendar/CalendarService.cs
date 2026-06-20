using Data;
using CatalogAPI.Features.Games.Common.Dtos;
using CatalogAPI.Features.Games.Common.Projections;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Calendar;

internal sealed class CalendarService(AppDbContext db) : ICalendarService
{
    public async Task<List<GameBrowseDto>> GetGamesCalendarAsync(int year, CancellationToken ct)
    {
        var yearStart = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var nextYearStart = yearStart.AddDays(DateTime.IsLeapYear(year) ? 366 : 365);

        return await db.GamesSearches
            .Where(e => e.FirstReleaseDateUtc >= yearStart && e.FirstReleaseDateUtc < nextYearStart)
            .OrderByDescending(g => g.Hypes ?? 0)
            .Join(db.Games, gs => gs.Id, g => g.Id, (_, game) => game)
            .Take(50)
            .SelectGameBrowseDto()
            .ToListAsync(ct);
    }
}
