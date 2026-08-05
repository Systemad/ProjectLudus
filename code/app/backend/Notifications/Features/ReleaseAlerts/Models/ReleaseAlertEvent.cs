namespace Notifications.Features.ReleaseAlerts.Models;

public sealed class ReleaseAlertEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long GameId { get; set; }
    public DateOnly ReleaseDayUtc { get; set; }
    public DateTimeOffset DispatchAtUtc { get; set; }
    public ReleaseAlertEventStatus Status { get; set; } = ReleaseAlertEventStatus.Scheduled;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public List<ReleaseAlertPlatform> Platforms { get; set; } = [];
    public List<NotificationDelivery> Deliveries { get; set; } = [];
}
