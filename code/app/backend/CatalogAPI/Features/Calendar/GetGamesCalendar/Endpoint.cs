using System.ComponentModel.DataAnnotations;


namespace CatalogAPI.Features.Calendar.GetGamesCalendar;

public static class Endpoint
{
    /// <summary>Games calendar for a specific year.</summary>
    public static async Task<IResult> HandleAsync(
        [Range(1, 9999)] int year,
        ICalendarService calendarService,
        CancellationToken cancellationToken
    )
    {
        var games = await calendarService.GetGamesCalendarAsync(year, cancellationToken);
        return Results.Ok(new GetGamesCalendarResponse { Year = year, Games = games });
    }
}
