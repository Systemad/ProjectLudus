using PlayAPI.Features.Games.Analytics.RecordClick;

namespace PlayAPI.Features.Games.Analytics;

public static class MapAnalyticEndpoints
{
    public static IEndpointRouteBuilder MapGamesAnalyticsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRecordClickEndpoints();
        return app;
    }
}
