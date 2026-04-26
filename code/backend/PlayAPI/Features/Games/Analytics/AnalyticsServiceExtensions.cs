using PlayAPI.Features.Games.Analytics.RecordEvent;

namespace PlayAPI.Features.Games.Analytics;

public static class AnalyticsServiceExtensions
{
    public static IServiceCollection AddGamesAnalyticsServices(this IServiceCollection services)
    {
        services.AddScoped<RecordGameEventService>();
        return services;
    }
}
