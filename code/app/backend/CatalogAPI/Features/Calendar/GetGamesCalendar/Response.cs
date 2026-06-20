namespace CatalogAPI.Features.Calendar.GetGamesCalendar;

public sealed record GetGamesCalendarResponse
{
    public required int Year { get; init; }
    public required List<GameBrowseDto> Games { get; init; } = [];
}