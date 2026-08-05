using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.Calendar;

public static class Map
{
    public static IEndpointRouteBuilder MapCalendarFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/calendar").CacheOutput("DefaultCache");

        group
            .MapGet("/{year:int}", GetGamesCalendar.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Calendar}/GetGames")
            .WithTags(EndpointMetadata.Calendar)
            .Produces<PagedGamesResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return app;
    }
}
