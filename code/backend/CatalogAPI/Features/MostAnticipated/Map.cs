using CatalogAPI.Features.MostAnticipated.GetMostAnticipated;

namespace CatalogAPI.Features.MostAnticipated;

public static class Map
{
    public static IEndpointRouteBuilder MapMostAnticipatedFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/most-anticipated").CacheOutput("DefaultCache");

        group
            .MapGet("/", GetMostAnticipatedEndpoint.HandleAsync)
            .WithName("MostAnticipated/Get")
            .WithTags("MostAnticipated")
            .Produces<GetMostAnticipatedEndpoint.MostAnticipatedResponse>(StatusCodes.Status200OK);

        return app;
    }
}
