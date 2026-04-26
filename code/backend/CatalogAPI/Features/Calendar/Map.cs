using CatalogAPI.Features.Calendar.GetGamesCalendar;

namespace CatalogAPI.Features.Calendar;

public static class Map
{
    public static IEndpointRouteBuilder MapCalendarFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/calendar").CacheOutput("DefaultCache");

        group
            .MapGet("/{year:int}", GetGamesCalendarEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Calendar}/GetGames")
            .WithTags(EndpointMetadata.Calendar)
            .Produces<GetGamesCalendarEndpoint.Response>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return app;
    }
}
