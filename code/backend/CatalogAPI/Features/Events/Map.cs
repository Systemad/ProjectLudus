using CatalogAPI.Features.Events.GetById;
using CatalogAPI.Features.Events.GetByYear;

namespace CatalogAPI.Features.Events;

public static class Map
{
    public static IEndpointRouteBuilder MapEventsFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/events").CacheOutput("DefaultCache");

        group
            .MapGet("/year/{year:int}", GetEventsByYearEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Events}/GetByYear")
            .WithTags(EndpointMetadata.Events)
            .Produces<GetEventsByYearEndpoint.Response>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group
            .MapGet("/{id:long}", GetEventByIdEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Events}/GetById")
            .WithTags(EndpointMetadata.Events)
            .Produces<GetEventByIdEndpoint.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
