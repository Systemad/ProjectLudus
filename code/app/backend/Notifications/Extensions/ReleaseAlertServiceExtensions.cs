using Data.Context;
using Microsoft.EntityFrameworkCore;
using Notifications.Features.ReleaseAlerts;
using Notifications.Infrastructure.Persistence;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.Customizer;
using TickerQ.EntityFrameworkCore.DependencyInjection;

namespace Notifications.Extensions;

public static class ReleaseAlertServiceExtensions
{
    public static WebApplicationBuilder AddReleaseAlerts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder
                .UseNpgsql(builder.Configuration.GetConnectionString("catalogdb"))
                .UseSnakeCaseNamingConvention();
        });
        builder.Services.AddDbContext<NotificationDbContext>(optionsBuilder =>
        {
            optionsBuilder
                .UseNpgsql(builder.Configuration.GetConnectionString("notificationsdb"))
                .UseSnakeCaseNamingConvention();
        });
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddScoped<ReleaseAlertDiscovery>();
        builder.Services.AddScoped<ReleaseAlertDispatchService>();
        builder.Services.AddTickerQ(options =>
        {
            options.AddOperationalStore(ef =>
            {
                ef.UseApplicationDbContext<NotificationDbContext>(
                    ConfigurationType.UseModelCustomizer
                );
            });
        });
        builder.Services.MapTicker<DiscoverUpcomingReleaseAlertsJob>().WithCron("5 0 * * *");

        return builder;
    }
}
