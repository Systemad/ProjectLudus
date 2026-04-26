using PlayAPI.Features.Games.Analytics.GameMetrics;
using PlayAPI.Features.Games.Analytics.RecordEvent;

namespace PlayAPI.Features.Games.Analytics;

public static class MapAnalyticEndpoints
{
    public static IEndpointRouteBuilder MapGamesAnalyticsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRecordGameEventEndpoints();
        app.MapGetGameMetricEndpoints();
        return app;
    }
}
