using CatalogAPI.Features.PopularityTypes.GetById;

namespace CatalogAPI.Features.PopularityTypes;

public static class Map
{
    public static IEndpointRouteBuilder MapPopularityTypesFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/popularity").CacheOutput("DefaultCache");

        group
            .MapGet("/{popularityTypeId:long}", GetByIdEndpoints.HandleAsync)
            .WithName("PopularityTypes/GetById")
            .WithTags("PopularityTypes")
            .Produces<GetByIdEndpoints.GetByIdResponse>(StatusCodes.Status200OK);

        return app;
    }
}
