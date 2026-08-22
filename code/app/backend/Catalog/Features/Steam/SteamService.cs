using Catalog.Features.Games.Common.Pagination;
using Catalog.Features.Games.Common.Projections;
using Catalog.Features.Steam.Charts.GetChart;
using Catalog.Features.Steam.Charts.GetConcurrentUsersChart;
using Catalog.Features.Steam.Store.GetPricing;
using Catalog.Features.Steam.Store.GetReviews;
using Data.Models;

namespace Catalog.Features.Steam;

internal sealed class SteamService(AppDbContext db) : ISteamService
{
    public async Task<GetPricingResponse?> GetPricingAsync(long gameId, CancellationToken ct)
    {
        var pricing = await db
            .SteamLatestPricings.Where(s => s.GameId == gameId && s.SteamAppId.HasValue)
            .Select(s => new
            {
                s.GameId,
                s.SteamAppId,
                s.FinalCents,
                s.DiscountPercent,
                s.Currency,
                s.High30d,
                s.Low30d,
            })
            .FirstOrDefaultAsync(ct);

        return pricing is null || pricing.SteamAppId is null
            ? null
            : new GetPricingResponse(
                pricing.GameId.ToString(),
                pricing.SteamAppId.Value.ToString(),
                pricing.FinalCents,
                pricing.DiscountPercent,
                pricing.Currency,
                pricing.High30d,
                pricing.Low30d
            );
    }

    public async Task<GetReviewsResponse?> GetReviewsAsync(long gameId, CancellationToken ct)
    {
        var reviews = await db
            .SteamReviews.Where(r => r.GameId == gameId && r.SteamAppId.HasValue)
            .Select(r => new
            {
                r.GameId,
                r.SteamAppId,
                r.NumReviews,
                r.ReviewScore,
                r.ReviewScoreDesc,
                r.TotalPositive,
                r.TotalNegative,
                r.TotalReviews,
            })
            .FirstOrDefaultAsync(ct);

        return reviews is null || reviews.SteamAppId is null
            ? null
            : new GetReviewsResponse(
                reviews.GameId.ToString(),
                reviews.SteamAppId.Value.ToString(),
                reviews.NumReviews,
                reviews.ReviewScore,
                reviews.ReviewScoreDesc,
                reviews.TotalPositive,
                reviews.TotalNegative,
                reviews.TotalReviews
            );
    }

    public async Task<PagedGamesResponse> GetChartAsync(Request request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var page = request.PageNumber;
        var pageSize = request.Size;
        var skip = (page - 1) * pageSize;

        var query = (request.Type ?? Request.TypeMostPlayed).ToLowerInvariant() switch
        {
            Request.TypePopularReleases => PopularReleasesQuery(db, now),
            Request.TypeHotReleases => HotReleasesQuery(db, now),
            _ => MostPlayedQuery(db),
        };

        var games = await query.Skip(skip).Take(pageSize + 1).SelectGameBrowseDto().ToListAsync(ct);

        return PagedGamesResponse.Create(games, page, pageSize);
    }

    public async Task<ConcurrentUsersChartResponse?> GetConcurrentUsersChartAsync(
        long gameId,
        string? range,
        CancellationToken ct
    )
    {
        static DateTime Floor(DateTime dt, TimeSpan interval) =>
            new DateTime(dt.Ticks / interval.Ticks * interval.Ticks, DateTimeKind.Utc);

        var (interval, bucketSize, days) = range switch
        {
            "24h" => (TimeSpan.FromHours(1), "1h", 1),
            "48h" => (TimeSpan.FromHours(1), "1h", 2),
            "7d" or null => (TimeSpan.FromHours(1), "1h", 7),
            "30d" => (TimeSpan.FromDays(1), "1d", 30),
            "90d" => (TimeSpan.FromDays(1), "1d", 90),
            "1y" => (TimeSpan.FromDays(1), "1d", 365),
            _ => (TimeSpan.FromHours(1), "1h", 7),
        };

        var end = Floor(DateTime.UtcNow, interval);
        var since = end.AddDays(-days);

        List<ChartPointDto> points;

        if (interval == TimeSpan.FromHours(1))
        {
            points = await db
                .SteamPlayerStatsHourlies.Where(x => x.GameId == gameId && x.Bucket >= since)
                .OrderBy(x => x.Bucket)
                .Select(x => new ChartPointDto(
                    x.Bucket!.Value,
                    x.PeakPlayers ?? 0
                ))
                .ToListAsync(ct);
        }
        else
        {
            points = await db
                .SteamPlayerStatsDailies.Where(x => x.GameId == gameId && x.Bucket >= since)
                .OrderBy(x => x.Bucket)
                .Select(x => new ChartPointDto(
                    x.Bucket!.Value,
                    x.PeakPlayers ?? 0
                ))
                .ToListAsync(ct);
        }

        var fillEnd = points.Count > 0 ? points[^1].Timestamp : end;
        var lookup = points.ToDictionary(x => x.Timestamp);
        var filled = new List<ChartPointDto>();

        for (var t = since; t <= fillEnd; t += interval)
        {
            filled.Add(lookup.TryGetValue(t, out var p) ? p : new ChartPointDto(t, 0));
        }

        return new ConcurrentUsersChartResponse(range ?? "7d", bucketSize, filled);
    }

    private static IQueryable<Game> MostPlayedQuery(AppDbContext db)
    {
        return db
            .Games.Where(game => game.SteamLatestPlayerCount != null)
            .OrderByDescending(game => game.SteamLatestPlayerCount!.CurrentPlayers)
            .ThenBy(game => game.Id);
    }

    private static IQueryable<Game> PopularReleasesQuery(AppDbContext db, DateTime now)
    {
        var twoWeeksAgo = now.AddDays(-14);
        return db
            .Games.Where(g => g.FirstReleaseDateUtc >= twoWeeksAgo && g.FirstReleaseDateUtc <= now)
            .OrderByDescending(game => game.SteamLatestPlayerCount!.CurrentPlayers)
            .ThenBy(game => game.Id);
    }

    private static IQueryable<Game> HotReleasesQuery(AppDbContext db, DateTime now)
    {
        var twoWeeksAgo = now.AddDays(-14);
        return db
            .Games.Where(g => g.FirstReleaseDateUtc >= twoWeeksAgo && g.FirstReleaseDateUtc <= now)
            .OrderByDescending(game => game.SteamReview!.TotalPositive)
            .ThenBy(game => game.Id);
    }
}
