using CatalogAPI.Features.Stats.GetStats;

namespace CatalogAPI.Features.Stats;

public static class Map
{
    public static IEndpointRouteBuilder MapStatsFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/stats").CacheOutput("DefaultCache");

        group
            .MapGet("/", GetStatsEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Stats}/GetStats")
            .WithTags(EndpointMetadata.Stats)
            .Produces<GetStatsEndpoint.GetStatsResponse>(StatusCodes.Status200OK);

        return app;
    }
}
