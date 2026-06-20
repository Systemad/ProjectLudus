namespace CatalogAPI.Features.Homepage;

public static class Map
{
    public static IEndpointRouteBuilder MapHomepageFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/homepage").CacheOutput("DefaultCache");

        group
            .MapGet("/upcoming", GetUpcoming.Endpoint.HandleAsync)
            .WithName("Homepage/GetUpcoming")
            .WithTags(EndpointMetadata.Homepage)
            .Produces<GetUpcoming.UpcomingResponse>();

        group
            .MapGet("/popularity-tables", GetPopularityTables.Endpoint.HandleAsync)
            .WithName("Homepage/GetPopularityTables")
            .WithTags(EndpointMetadata.Homepage)
            .Produces<GetPopularityTables.PopularityTablesResponse>();

        return app;
    }
}
