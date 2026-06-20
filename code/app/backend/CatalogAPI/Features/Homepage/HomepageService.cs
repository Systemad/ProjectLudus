using Data;
using CatalogAPI.Features.Games.Common.Projections;
using CatalogAPI.Features.Games.Common.Dtos;
using CatalogAPI.Features.Homepage.GetPopularityTables;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Homepage;

internal sealed class HomepageService(AppDbContext db) : IHomepageService
{
    public async Task<List<GameBrowseDto>> GetUpcomingAsync(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var oneMonthFromNow = now.AddDays(30);

        return await db.Games
            .Where(g => g.FirstReleaseDateUtc >= now && g.FirstReleaseDateUtc <= oneMonthFromNow)
            .Where(g => g.GameTypeNavigation!.Type == "Main Game")
            .OrderByDescending(g =>
                (db.Popscores.Where(p => p.GameId == g.Id && p.PopularityType == 10)
                    .Select(p => (double?)p.Value).Max() ?? 0) * 0.5
                + (g.Hypes ?? 0) * 0.3
                + (db.Popscores.Where(p => p.GameId == g.Id && p.PopularityType == 1)
                    .Select(p => (double?)p.Value).Max() ?? 0) * 0.2)
            .Take(6)
            .SelectGameBrowseDto()
            .ToListAsync(ct);
    }

    public async Task<PopularityTablesResponse> GetPopularityTablesAsync(CancellationToken ct)
    {
        async Task<List<GameBrowseDto>> FetchTable(long typeId, int limit)
        {
            var latest = await db.Popscores.Where(p => p.PopularityType == typeId)
                .MaxAsync(p => (DateTime?)p.CapturedAt, ct);
            if (latest is null) return [];

            var ids = await db.Popscores.Where(p => p.PopularityType == typeId && p.CapturedAt == latest)
                .GroupBy(p => p.GameId).Select(g => new { GameId = g.Key, MaxScore = g.Max(p => p.Value) })
                .OrderByDescending(x => x.MaxScore).Take(limit).Select(x => x.GameId).ToListAsync(ct);

            if (ids.Count == 0) return [];
            return await db.Games.Where(g => ids.Contains(g.Id)).SelectGameBrowseDto().ToListAsync(ct);
        }

        var wishlisted = await FetchTable(10, 10);
        var sellers = await FetchTable(9, 10);
        return new PopularityTablesResponse(wishlisted, sellers);
    }
}
