using Notifications.Features.ReleaseAlerts;
using Notifications.Features.ReleaseAlerts.Models;

namespace Notifications.Tests.Features;

public sealed class ReleaseAlertScheduleTests
{
    [Test]
    [Arguments("YYYYMMDD", "Full Release", true)]
    [Arguments("YYYY", "Full Release", false)]
    [Arguments("YYYYMM", "Full Release", false)]
    [Arguments("YYYYQ4", "Full Release", false)]
    [Arguments("TBD", "Full Release", false)]
    [Arguments("YYYYMMDD", "Early Access", false)]
    [Arguments("YYYYMMDD", "Cancelled", false)]
    [Arguments("YYYYMMDD", null, false)]
    public async Task IsEligible_RequiresExactDateAndFullRelease(
        string? dateFormat,
        string? status,
        bool expected
    )
    {
        var result = ReleaseAlertSchedule.IsEligible(dateFormat, status);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task Group_CombinesPlatformsForTheSameGameAndDay()
    {
        var releaseDay = new DateOnly(2026, 8, 1);
        var groups = ReleaseAlertSchedule.Group([
            new ScheduledRelease(10, 42, releaseDay, 6, null),
            new ScheduledRelease(11, 42, releaseDay, 48, null),
            new ScheduledRelease(12, 42, releaseDay.AddDays(1), 6, null),
        ]);

        await Assert.That(groups).Count().IsEqualTo(2);
        await Assert.That(groups[0].Releases).Count().IsEqualTo(2);
        await Assert.That(groups[0].GameId).IsEqualTo(42);
        await Assert.That(groups[0].ReleaseDayUtc).IsEqualTo(releaseDay);
    }
}
