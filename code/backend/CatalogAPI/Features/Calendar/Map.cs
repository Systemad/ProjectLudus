using CatalogAPI.Features.Calendar.GetGamesCalendar;

namespace CatalogAPI.Features.Calendar;

public static class Map
{
    public static IEndpointRouteBuilder MapCalendarFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/calendar").CacheOutput("DefaultCache");

        group.MapGetGamesCalendarEndpoint();

        return app;
    }
}