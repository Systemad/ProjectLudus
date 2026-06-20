namespace CatalogAPI.Features.Calendar;

public interface ICalendarService
{
    Task<List<GameBrowseDto>> GetGamesCalendarAsync(int year, CancellationToken ct);
}
