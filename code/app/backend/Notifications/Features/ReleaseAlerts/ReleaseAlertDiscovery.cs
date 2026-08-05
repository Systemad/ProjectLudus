using Data.Context;
using Microsoft.EntityFrameworkCore;
using Notifications.Features.ReleaseAlerts.Models;
using Notifications.Infrastructure.Persistence;

namespace Notifications.Features.ReleaseAlerts;

public sealed class ReleaseAlertDiscovery(
    AppDbContext catalogDbContext,
    NotificationDbContext notificationDbContext,
    TimeProvider timeProvider
)
{
    public async Task<int> DiscoverAsync(CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var from = DateOnly.FromDateTime(now.UtcDateTime);
        var to = from.AddDays(3);
        var fromUnix = new DateTimeOffset(
            from.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
        ).ToUnixTimeSeconds();
        var toUnix = new DateTimeOffset(
            to.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
        ).ToUnixTimeSeconds();

        var releaseRows = await catalogDbContext
            .ReleaseDates.AsNoTracking()
            .Where(release =>
                release.Game.HasValue
                && release.Date.HasValue
                && release.Platform.HasValue
                && release.Date >= fromUnix
                && release.Date < toUnix
                && release.DateFormatNavigation != null
                && release.DateFormatNavigation.Format == ReleaseAlertSchedule.ExactDayFormat
                && release.StatusNavigation != null
                && release.StatusNavigation.Name == ReleaseAlertSchedule.FullReleaseStatus
            )
            .Select(release => new
            {
                release.Id,
                GameId = release.Game!.Value,
                ReleaseTimestamp = release.Date!.Value,
                PlatformId = release.Platform!.Value,
                release.ReleaseRegion,
            })
            .ToListAsync(cancellationToken);

        var releases = ReleaseAlertSchedule.Group(
            releaseRows.Select(release => new ScheduledRelease(
                release.Id,
                release.GameId,
                DateOnly.FromDateTime(
                    DateTimeOffset.FromUnixTimeSeconds(release.ReleaseTimestamp).UtcDateTime
                ),
                release.PlatformId,
                release.ReleaseRegion
            ))
        );

        var existing = await notificationDbContext
            .ReleaseAlertEvents.Include(release => release.Platforms)
            .Where(release => release.ReleaseDayUtc >= from && release.ReleaseDayUtc < to)
            .ToDictionaryAsync(
                release => new ReleaseAlertEventKey(release.GameId, release.ReleaseDayUtc),
                cancellationToken
            );

        var created = 0;
        foreach (var releaseGroup in releases)
        {
            var key = new ReleaseAlertEventKey(releaseGroup.GameId, releaseGroup.ReleaseDayUtc);
            if (!existing.TryGetValue(key, out var releaseAlert))
            {
                releaseAlert = new ReleaseAlertEvent
                {
                    GameId = key.GameId,
                    ReleaseDayUtc = key.ReleaseDayUtc,
                    DispatchAtUtc = new DateTimeOffset(
                        key.ReleaseDayUtc.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
                    ),
                };
                notificationDbContext.ReleaseAlertEvents.Add(releaseAlert);
                existing.Add(key, releaseAlert);
                created++;
            }

            var knownSourceIds = releaseAlert
                .Platforms.Select(platform => platform.SourceReleaseDateId)
                .ToHashSet();
            foreach (
                var candidate in releaseGroup.Releases.Where(candidate =>
                    knownSourceIds.Add(candidate.SourceReleaseDateId)
                )
            )
            {
                releaseAlert.Platforms.Add(
                    new ReleaseAlertPlatform
                    {
                        SourceReleaseDateId = candidate.SourceReleaseDateId,
                        PlatformId = candidate.PlatformId,
                        ReleaseRegionId = candidate.ReleaseRegionId,
                    }
                );
            }
        }

        await notificationDbContext.SaveChangesAsync(cancellationToken);
        return created;
    }

    private sealed record ReleaseAlertEventKey(long GameId, DateOnly ReleaseDayUtc);
}
