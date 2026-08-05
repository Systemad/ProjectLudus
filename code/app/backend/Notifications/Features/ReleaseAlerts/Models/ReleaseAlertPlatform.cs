namespace Notifications.Features.ReleaseAlerts.Models;

public sealed class ReleaseAlertPlatform
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReleaseAlertEventId { get; set; }
    public ReleaseAlertEvent ReleaseAlertEvent { get; set; } = null!;
    public long SourceReleaseDateId { get; set; }
    public long PlatformId { get; set; }
    public long? ReleaseRegionId { get; set; }
}
