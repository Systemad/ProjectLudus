using CatalogAPI.Features.Stats.GetStats;

namespace CatalogAPI.Features.Stats;

public static class Map
{
    public static IEndpointRouteBuilder MapStatsFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stats").CacheOutput("DefaultCache");

        group.MapGetStatsEndpoint();

        return app;
    }
}
