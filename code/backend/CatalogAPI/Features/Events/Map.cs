using CatalogAPI.Features.Events.GetById;
using CatalogAPI.Features.Events.GetByYear;

namespace CatalogAPI.Features.Events;

public static class Map
{
    public static IEndpointRouteBuilder MapEventsFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/events").CacheOutput("DefaultCache");

        group.MapGetEventsByYearEndpoint();
        group.MapGetEventByIdEndpoint();

        return app;
    }
}
