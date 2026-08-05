using Microsoft.EntityFrameworkCore;
using Notifications.Features.ReleaseAlerts.Models;
using Notifications.Features.ReleaseAlerts.Notifications;
using Notifications.Infrastructure.Persistence;

namespace Notifications.Features.ReleaseAlerts;

public sealed class ReleaseAlertDispatchService(
    NotificationDbContext notificationDbContext,
    TimeProvider timeProvider
)
{
    public async Task DispatchAsync(
        ReleaseAlertEvent releaseAlert,
        IReadOnlyCollection<ReleaseAlertRecipient> recipients,
        INotificationSender sender,
        CancellationToken cancellationToken
    )
    {
        var endpointIds = recipients.Select(recipient => recipient.PushEndpointId).ToList();
        var deliveredEndpoints = await notificationDbContext
            .NotificationDeliveries.Where(delivery =>
                delivery.ReleaseAlertEventId == releaseAlert.Id
                && delivery.Provider == NotificationProvider.ExpoPush
                && endpointIds.Contains(delivery.PushEndpointId)
            )
            .Select(delivery => delivery.PushEndpointId)
            .ToHashSetAsync(cancellationToken);

        var deliveries = recipients
            .Where(recipient => deliveredEndpoints.Add(recipient.PushEndpointId))
            .Select(recipient => new NotificationDelivery
            {
                ReleaseAlertEventId = releaseAlert.Id,
                UserId = recipient.UserId,
                PushEndpointId = recipient.PushEndpointId,
                Provider = NotificationProvider.ExpoPush,
            })
            .ToList();

        notificationDbContext.NotificationDeliveries.AddRange(deliveries);
        await notificationDbContext.SaveChangesAsync(cancellationToken);

        foreach (var delivery in deliveries)
        {
            var recipient = new ReleaseAlertRecipient(delivery.UserId, delivery.PushEndpointId);
            var result = await sender.SendAsync(recipient, releaseAlert, cancellationToken);
            delivery.Status = result.Succeeded
                ? NotificationDeliveryStatus.Sent
                : NotificationDeliveryStatus.Failed;
            delivery.SentAtUtc = result.Succeeded ? timeProvider.GetUtcNow() : null;
            notificationDbContext.NotificationDeliveryAttempts.Add(
                new NotificationDeliveryAttempt
                {
                    NotificationDeliveryId = delivery.Id,
                    Succeeded = result.Succeeded,
                    ProviderReceipt = result.ProviderReceipt,
                    FailureReason = result.FailureReason,
                }
            );
        }

        await notificationDbContext.SaveChangesAsync(cancellationToken);
    }
}
