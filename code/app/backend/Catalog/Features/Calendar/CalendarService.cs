using Catalog.Features.Games.Common.Pagination;
using Catalog.Features.Games.Common.Projections;

namespace Catalog.Features.Calendar;

internal sealed class CalendarService(AppDbContext db) : ICalendarService
{
    public async Task<PagedGamesResponse> GetGamesCalendarAsync(
        int year,
        int page,
        int pageSize,
        CancellationToken ct
    )
    {
        var from = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var to = from.AddYears(1);
        var skip = (page - 1) * pageSize;

        var games = await db
            .Games.Where(game => game.FirstReleaseDateUtc >= from && game.FirstReleaseDateUtc < to)
            .OrderBy(game => game.FirstReleaseDateUtc)
            .ThenByDescending(game => game.Hypes)
            .ThenBy(game => game.Id)
            .Skip(skip)
            .Take(pageSize + 1)
            .SelectGameBrowseDto()
            .ToListAsync(ct);

        return PagedGamesResponse.Create(games, page, pageSize);
    }
}
