using PlayAPI.Features.Games.Analytics.RecordClick;

namespace PlayAPI.Features.Games.Analytics;

public static class AnalyticsServiceExtensions
{
    public static IServiceCollection AddGamesAnalyticsServices(this IServiceCollection services)
    {
        services.AddScoped<GameClickTrackingService>();
        return services;
    }
}
