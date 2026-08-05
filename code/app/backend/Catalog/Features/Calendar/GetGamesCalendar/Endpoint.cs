using System.ComponentModel.DataAnnotations;
using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.Calendar.GetGamesCalendar;

public static class Endpoint
{
    public static async Task<Ok<PagedGamesResponse>> HandleAsync(
        [Range(1, 9999)] int year,
        [AsParameters] PageRequest request,
        ICalendarService calendarService,
        CancellationToken cancellationToken
    )
    {
        var games = await calendarService.GetGamesCalendarAsync(
            year,
            request.PageNumber,
            request.Size,
            cancellationToken
        );

        return TypedResults.Ok(games);
    }
}
