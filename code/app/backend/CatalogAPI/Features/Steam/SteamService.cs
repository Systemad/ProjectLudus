using Data;
using CatalogAPI.Features.Games.Common.Projections;
using CatalogAPI.Features.Steam.Charts.GetChart;
using CatalogAPI.Features.Steam.Charts.GetConcurrentUsersChart;
using CatalogAPI.Features.Steam.Store.GetPricing;
using CatalogAPI.Features.Steam.Store.GetReviews;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Steam;

internal sealed class SteamService(AppDbContext db) : ISteamService
{
    public async Task<GetPricingResponse?> GetPricingAsync(
        long gameId, CancellationToken ct)
    {
        return await db.SteamLatestPricings
            .Where(s => s.GameId == gameId)
            .Select(s => new GetPricingResponse(
                s.GameId, s.SteamAppId, s.FinalCents, s.DiscountPercent,
                s.Currency, s.High30d, s.Low30d))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<GetReviewsResponse?> GetReviewsAsync(
        long gameId, CancellationToken ct)
    {
        return await db.SteamReviews
            .Where(r => r.GameId == gameId)
            .Select(r => new GetReviewsResponse(
                r.GameId, r.SteamAppId, r.NumReviews, r.ReviewScore,
                r.ReviewScoreDesc, r.TotalPositive, r.TotalNegative, r.TotalReviews))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<GameBrowseDto>> GetChartAsync(
        Request request, CancellationToken ct)
    {
        var take = Math.Clamp(request.Limit ?? 10, 1, 100);
        var now = DateTime.UtcNow;

        IQueryable<Data.Game> query = (request.Type ?? Request.TypeMostPlayed).ToLowerInvariant() switch
        {
            Request.TypeMostPlayed => MostPlayedQuery(db, take),
            Request.TypePopularReleases => PopularReleasesQuery(db, take, now),
            Request.TypeHotReleases => HotReleasesQuery(db, take, now),
            _ => MostPlayedQuery(db, take),
        };

        return await query.SelectGameBrowseDto().ToListAsync(ct);
    }

    public async Task<ConcurrentUsersChartResponse?> GetConcurrentUsersChartAsync(
        long gameId, string? range, CancellationToken ct)
    {
        static DateTime Floor(DateTime dt, TimeSpan interval) =>
            new DateTime(dt.Ticks / interval.Ticks * interval.Ticks, DateTimeKind.Utc);

        var (interval, bucketSize, days) = range switch
        {
            "48h" => (TimeSpan.FromHours(1), "1h", 2),
            "7d" or null => (TimeSpan.FromHours(1), "1h", 7),
            "30d" => (TimeSpan.FromDays(1), "1d", 30),
            "90d" => (TimeSpan.FromDays(1), "1d", 90),
            "1y" => (TimeSpan.FromDays(1), "1d", 365),
            _ => (TimeSpan.FromHours(1), "1h", 7)
        };

        var end = Floor(DateTime.UtcNow, interval);
        var since = end.AddDays(-days);

        List<ChartPointDto> points;

        if (interval == TimeSpan.FromHours(1))
        {
            points = await db.SteamPlayerStatsHourlies
                .Where(x => x.GameId == gameId && x.Bucket >= since)
                .OrderBy(x => x.Bucket)
                .Select(x => new ChartPointDto(
                    x.Bucket!.Value, x.PeakPlayers ?? 0, x.AvgPlayers ?? 0))
                .ToListAsync(ct);
        }
        else
        {
            points = await db.SteamPlayerStatsDailies
                .Where(x => x.GameId == gameId && x.Bucket >= since)
                .OrderBy(x => x.Bucket)
                .Select(x => new ChartPointDto(
                    x.Bucket!.Value, x.PeakPlayers ?? 0, x.AvgPlayers ?? 0))
                .ToListAsync(ct);
        }

        var lookup = points.ToDictionary(x => x.Timestamp);
        var filled = new List<ChartPointDto>();

        for (var t = since; t <= end; t += interval)
        {
            filled.Add(lookup.TryGetValue(t, out var p)
                ? p
                : new ChartPointDto(t, 0, 0));
        }

        return new ConcurrentUsersChartResponse(
            range ?? "7d", bucketSize, filled);
    }

    private static IQueryable<Data.Game> MostPlayedQuery(AppDbContext db, int take)
    {
        var gameIds = db.SteamLatestPlayerCounts
            .OrderByDescending(s => s.CurrentPlayers).Take(take).Select(s => s.GameId);
        return db.Games.Where(g => gameIds.Contains(g.Id));
    }

    private static IQueryable<Data.Game> PopularReleasesQuery(AppDbContext db, int take, DateTime now)
    {
        var twoWeeksAgo = now.AddDays(-14);
        return db.Games
            .Where(g => g.FirstReleaseDateUtc >= twoWeeksAgo && g.FirstReleaseDateUtc <= now)
            .GroupJoin(db.SteamLatestPlayerCounts.Select(s => new { s.GameId, MaxPlayers = s.CurrentPlayers ?? 0L }),
                g => g.Id, s => s.GameId,
                (g, s) => new { Game = g, MaxPlayers = s.Select(x => x.MaxPlayers).FirstOrDefault() })
            .OrderByDescending(x => x.MaxPlayers).Take(take).Select(x => x.Game);
    }

    private static IQueryable<Data.Game> HotReleasesQuery(AppDbContext db, int take, DateTime now)
    {
        var twoWeeksAgo = now.AddDays(-14);
        return db.Games
            .Where(g => g.FirstReleaseDateUtc >= twoWeeksAgo && g.FirstReleaseDateUtc <= now)
            .GroupJoin(db.SteamReviews.Select(r => new { r.GameId, Positive = r.TotalPositive ?? 0 }),
                g => g.Id, r => r.GameId,
                (g, r) => new { Game = g, Positive = r.Select(x => x.Positive).FirstOrDefault() })
            .OrderByDescending(x => x.Positive).Take(take).Select(x => x.Game);
    }
}
