using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Features.ReleaseAlerts;
using Notifications.Features.ReleaseAlerts.Models;
using Notifications.Features.ReleaseAlerts.Notifications;
using Notifications.Infrastructure.Persistence;
using Notifications.Tests.Setup;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.Customizer;
using TickerQ.EntityFrameworkCore.DependencyInjection;

namespace Notifications.Tests.Features;

public sealed class ReleaseAlertDispatchServiceTests
{
    [ClassDataSource<NotificationDatabaseContainer>(Shared = SharedType.PerTestSession)]
    public required NotificationDatabaseContainer Database { get; init; }

    [Test]
    public async Task DispatchAsync_DeduplicatesEndpointsAndStoresSuccessfulAttempt()
    {
        await using var serviceProvider = CreateServiceProvider();
        await using var notificationDbContext =
            serviceProvider.GetRequiredService<NotificationDbContext>();
        await notificationDbContext.Database.MigrateAsync();
        var releaseAlert = new ReleaseAlertEvent
        {
            GameId = 730,
            ReleaseDayUtc = new DateOnly(2026, 8, 1),
            DispatchAtUtc = new DateTimeOffset(2026, 8, 1, 0, 0, 0, TimeSpan.Zero),
        };
        notificationDbContext.ReleaseAlertEvents.Add(releaseAlert);
        await notificationDbContext.SaveChangesAsync();
        var sender = new RecordingNotificationSender();
        var service = new ReleaseAlertDispatchService(notificationDbContext, TimeProvider.System);
        var recipient = new ReleaseAlertRecipient(Guid.NewGuid(), Guid.NewGuid());

        await service.DispatchAsync(
            releaseAlert,
            [recipient, recipient],
            sender,
            CancellationToken.None
        );

        await Assert.That(sender.Recipients).Count().IsEqualTo(1);
        await Assert.That(notificationDbContext.NotificationDeliveries).Count().IsEqualTo(1);
        var delivery = await notificationDbContext
            .NotificationDeliveries.Include(item => item.Attempts)
            .FirstAsync();
        await Assert.That(delivery.Status).IsEqualTo(NotificationDeliveryStatus.Sent);
        await Assert.That(delivery.Attempts).Count().IsEqualTo(1);
        await Assert.That(delivery.Attempts[0].Succeeded).IsTrue();
    }

    private ServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddDbContext<NotificationDbContext>(optionsBuilder =>
        {
            optionsBuilder
                .UseNpgsql(Database.Container.GetConnectionString())
                .UseSnakeCaseNamingConvention();
        });
        services.AddTickerQ(options =>
        {
            options.AddOperationalStore(ef =>
            {
                ef.UseApplicationDbContext<NotificationDbContext>(
                    ConfigurationType.UseModelCustomizer
                );
            });
        });

        return services.BuildServiceProvider();
    }

    private sealed class RecordingNotificationSender : INotificationSender
    {
        public List<ReleaseAlertRecipient> Recipients { get; } = [];

        public Task<NotificationSendResult> SendAsync(
            ReleaseAlertRecipient recipient,
            ReleaseAlertEvent releaseAlert,
            CancellationToken cancellationToken
        )
        {
            Recipients.Add(recipient);
            return Task.FromResult(new NotificationSendResult(true, "test-receipt", null));
        }
    }
}
