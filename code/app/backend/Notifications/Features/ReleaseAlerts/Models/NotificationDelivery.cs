namespace Notifications.Features.ReleaseAlerts.Models;

public sealed class NotificationDelivery
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReleaseAlertEventId { get; set; }
    public ReleaseAlertEvent ReleaseAlertEvent { get; set; } = null!;
    public Guid UserId { get; set; }
    public Guid PushEndpointId { get; set; }
    public NotificationProvider Provider { get; set; }
    public NotificationDeliveryStatus Status { get; set; } = NotificationDeliveryStatus.Pending;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? SentAtUtc { get; set; }
    public List<NotificationDeliveryAttempt> Attempts { get; set; } = [];
}
