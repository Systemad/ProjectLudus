namespace Notifications.Features.ReleaseAlerts.Notifications;

public sealed record ReleaseAlertRecipient(Guid UserId, Guid PushEndpointId);
