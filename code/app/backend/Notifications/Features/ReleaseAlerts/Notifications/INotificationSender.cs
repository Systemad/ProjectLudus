using Notifications.Features.ReleaseAlerts.Models;

namespace Notifications.Features.ReleaseAlerts.Notifications;

public interface INotificationSender
{
    Task<NotificationSendResult> SendAsync(
        ReleaseAlertRecipient recipient,
        ReleaseAlertEvent releaseAlert,
        CancellationToken cancellationToken
    );
}
