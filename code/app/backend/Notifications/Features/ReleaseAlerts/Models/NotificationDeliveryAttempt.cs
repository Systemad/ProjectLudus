namespace Notifications.Features.ReleaseAlerts.Models;

public sealed class NotificationDeliveryAttempt
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid NotificationDeliveryId { get; set; }
    public NotificationDelivery NotificationDelivery { get; set; } = null!;
    public DateTimeOffset AttemptedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public bool Succeeded { get; set; }
    public string? ProviderReceipt { get; set; }
    public string? FailureReason { get; set; }
}
