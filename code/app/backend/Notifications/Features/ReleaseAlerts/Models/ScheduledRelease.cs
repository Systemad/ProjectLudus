namespace Notifications.Features.ReleaseAlerts.Models;

public sealed record ScheduledRelease(
    long SourceReleaseDateId,
    long GameId,
    DateOnly ReleaseDayUtc,
    long PlatformId,
    long? ReleaseRegionId
);
