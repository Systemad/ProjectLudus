using Catalog.Features.Events.Dtos;
using Catalog.Features.Games.Common.Projections;
using Data.Models;

namespace Catalog.Features.Events;

internal sealed class EventService(AppDbContext db) : IEventService
{
    public async Task<List<EventDto>> GetListAsync(
        int? year,
        int? month,
        string? status,
        int? limit,
        CancellationToken ct
    )
    {
        year ??= DateTime.UtcNow.Year;
        var yearStart = new DateTime(year.Value, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var nextYearStart = yearStart.AddDays(DateTime.IsLeapYear(year.Value) ? 366 : 365);
        var take = Math.Clamp(limit ?? 100, 1, 200);
        var now = DateTime.UtcNow;

        IQueryable<Event> query = db.Events.Where(e =>
            e.StartTimeUtc >= yearStart && e.StartTimeUtc < nextYearStart
        );

        if (month.HasValue)
            query = query.Where(e => e.StartTimeUtc!.Value.Month == month.Value);

        query = status?.ToLowerInvariant() switch
        {
            "started" => query.Where(e => e.StartTimeUtc <= now),
            "finished" => query.Where(e =>
                e.EndTimeUtc != null ? e.EndTimeUtc <= now : e.StartTimeUtc <= now
            ),
            "notstarted" => query.Where(e => e.StartTimeUtc > now),
            _ => query,
        };

        var rawEvents = await query
            .OrderBy(e => e.StartTimeUtc)
            .Take(take)
            .AsSplitQuery()
            .Select(e => new
            {
                Event = e,
                GameIds = e
                    .Games.Where(g => g.FirstReleaseDateUtc.HasValue)
                    .Select(g => g.Id)
                    .ToList(),
            })
            .ToListAsync(ct);

        var allGameIds = rawEvents.SelectMany(r => r.GameIds).Distinct().ToList();

        var gameLookup =
            allGameIds.Count > 0
                ? await db
                    .Games.Where(g => allGameIds.Contains(g.Id))
                    .SelectGameBrowseDto()
                    .ToListAsync(ct)
                : [];

        var gameMap = gameLookup.ToDictionary(g => g.Id);

        return rawEvents
            .Select(r => new EventDto
            {
                Id = ApiId.Format(r.Event.Id),
                Name = r.Event.Name,
                Slug = r.Event.Slug,
                Description = r.Event.Description,
                LiveStreamUrl = r.Event.LiveStreamUrl,
                StartTimeUtc = r.Event.StartTimeUtc,
                EndTimeUtc = r.Event.EndTimeUtc,
                TimeZone = r.Event.TimeZone,
                LogoImageId = r.Event.EventLogoNavigation?.ImageId,
                Games = r
                    .GameIds.Select(gameId => gameMap.GetValueOrDefault(gameId.ToString()))
                    .Where(g => g != null)
                    .Select(g => g!)
                    .ToList(),
            })
            .ToList();
    }

    public async Task<EventDto?> GetByIdAsync(long id, CancellationToken ct)
    {
        var raw = await db
            .Events.Where(e => e.Id == id)
            .AsSplitQuery()
            .Select(e => new
            {
                Event = e,
                GameIds = e
                    .Games.Where(g => g.FirstReleaseDateUtc.HasValue)
                    .Select(g => g.Id)
                    .ToList(),
            })
            .FirstOrDefaultAsync(ct);

        if (raw is null)
            return null;

        var games =
            raw.GameIds.Count > 0
                ? await db
                    .Games.Where(g => raw.GameIds.Contains(g.Id))
                    .SelectGameBrowseDto()
                    .ToListAsync(ct)
                : [];

        return new EventDto
        {
            Id = ApiId.Format(raw.Event.Id),
            Name = raw.Event.Name,
            Slug = raw.Event.Slug,
            Description = raw.Event.Description,
            LiveStreamUrl = raw.Event.LiveStreamUrl,
            StartTimeUtc = raw.Event.StartTimeUtc,
            EndTimeUtc = raw.Event.EndTimeUtc,
            TimeZone = raw.Event.TimeZone,
            LogoImageId = raw.Event.EventLogoNavigation?.ImageId,
            Games = games,
        };
    }
}
