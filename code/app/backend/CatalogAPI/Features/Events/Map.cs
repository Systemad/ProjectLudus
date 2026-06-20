namespace CatalogAPI.Features.Events;

public static class Map
{
    public static IEndpointRouteBuilder MapEventsFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/events").CacheOutput("DefaultCache");

        group
            .MapGet("/", GetList.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Events}/GetList")
            .WithTags(EndpointMetadata.Events)
            .Produces<GetList.GetEventsListResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group
            .MapGet("/{id:long}", GetById.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Events}/GetById")
            .WithTags(EndpointMetadata.Events)
            .Produces<GetById.GetEventByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
