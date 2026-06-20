using Data;
using CatalogAPI.Features.Games.Common.Projections;
using CatalogAPI.Features.Games.Common.Dtos;
using CatalogAPI.Features.IGDB.GetPopscore;
using CatalogAPI.Features.IGDB.GetStatistics;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.IGDB;

internal sealed class IGDBService(AppDbContext db) : IIGDBService
{
    public async Task<List<GameBrowseDto>> GetMostAnticipatedAsync(int? limit, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var oneMonthFromNow = now.AddDays(30);
        var take = Math.Clamp(limit ?? 10, 1, 50);

        return await db.Games
            .Where(g => g.FirstReleaseDateUtc >= now && g.FirstReleaseDateUtc <= oneMonthFromNow)
            .Where(g => g.GameTypeNavigation!.Type == "Main Game")
            .OrderByDescending(g =>
                (db.Popscores.Where(p => p.GameId == g.Id && p.PopularityType == 10)
                    .Select(p => (double?)p.Value).Max() ?? 0) * 0.5
                + (g.Hypes ?? 0) * 0.3
                + (db.Popscores.Where(p => p.GameId == g.Id && p.PopularityType == 1)
                    .Select(p => (double?)p.Value).Max() ?? 0) * 0.2)
            .Take(take)
            .SelectGameBrowseDto()
            .ToListAsync(ct);
    }

    public async Task<GetPopscoreResponse> GetPopscoreAsync(
        long popularityTypeId, int limit, CancellationToken ct)
    {
        var latest = await db.Popscores
            .Where(p => p.PopularityType == popularityTypeId)
            .MaxAsync(p => (DateTime?)p.CapturedAt, ct);

        if (latest is null)
            return new GetPopscoreResponse([]);

        var games = await db.Popscores
            .Where(p => p.PopularityType == popularityTypeId && p.CapturedAt == latest)
            .GroupBy(p => p.GameId)
            .Select(g => new { GameId = g.Key, MaxScore = g.Max(p => p.Value) })
            .OrderByDescending(x => x.MaxScore)
            .Take(limit)
            .Join(db.Games, top => top.GameId, g => g.Id, (top, g) => g)
            .SelectGameBrowseDto()
            .ToListAsync(ct);

        return new GetPopscoreResponse(games);
    }

    public async Task<StatisticsResponse> GetStatisticsAsync(CancellationToken ct)
    {
        var totalGames = await db.Games.LongCountAsync(ct);
        var totalCompanies = await db.Companies.LongCountAsync(ct);
        var totalPlatforms = await db.Platforms.LongCountAsync(ct);
        var totalEvents = await db.Events.LongCountAsync(ct);

        return new StatisticsResponse(
            totalGames, totalCompanies, totalPlatforms, totalEvents);
    }
}
