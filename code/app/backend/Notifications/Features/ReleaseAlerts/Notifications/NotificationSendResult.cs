namespace Notifications.Features.ReleaseAlerts.Notifications;

public sealed record NotificationSendResult(
    bool Succeeded,
    string? ProviderReceipt,
    string? FailureReason
);
