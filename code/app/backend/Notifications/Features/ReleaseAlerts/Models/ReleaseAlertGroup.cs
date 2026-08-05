namespace Notifications.Features.ReleaseAlerts.Models;

public sealed record ReleaseAlertGroup(
    long GameId,
    DateOnly ReleaseDayUtc,
    IReadOnlyList<ScheduledRelease> Releases
);
