using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.Calendar;

public interface ICalendarService
{
    Task<PagedGamesResponse> GetGamesCalendarAsync(
        int year,
        int page,
        int pageSize,
        CancellationToken ct
    );
}
