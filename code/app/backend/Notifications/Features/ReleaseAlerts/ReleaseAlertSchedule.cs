using Notifications.Features.ReleaseAlerts.Models;

namespace Notifications.Features.ReleaseAlerts;

public static class ReleaseAlertSchedule
{
    public const string ExactDayFormat = "YYYYMMDD";
    public const string FullReleaseStatus = "Full Release";

    public static bool IsEligible(string? dateFormat, string? status)
    {
        return dateFormat == ExactDayFormat && status == FullReleaseStatus;
    }

    public static IReadOnlyList<ReleaseAlertGroup> Group(IEnumerable<ScheduledRelease> releases)
    {
        return releases
            .GroupBy(release => new { release.GameId, release.ReleaseDayUtc })
            .Select(group => new ReleaseAlertGroup(
                group.Key.GameId,
                group.Key.ReleaseDayUtc,
                group.DistinctBy(release => release.SourceReleaseDateId).ToList()
            ))
            .ToList();
    }
}
