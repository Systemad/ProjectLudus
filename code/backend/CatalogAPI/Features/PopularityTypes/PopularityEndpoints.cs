using CatalogAPI.Features.PopularityTypes.GetById;

namespace CatalogAPI.Features.PopularityTypes;

public static class PopularityEndpoints
{
    public static IEndpointRouteBuilder UsePopularityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetByIdEndpoints();
        return app;
    }
}
