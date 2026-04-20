using CatalogAPI.Features.PopularityTypes.GetById;

namespace CatalogAPI.Features.PopularityTypes;

public static class Map
{
    public static IEndpointRouteBuilder MapPopularityTypesFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/popularity").CacheOutput("DefaultCache");

        group.MapGetByIdEndpoint();

        return app;
    }
}