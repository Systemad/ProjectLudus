using Catalog.Features.Games.Common.Pagination;
using Catalog.Features.IGDB.GetStatistics;

namespace Catalog.Features.IGDB;

public interface IIGDBService
{
    Task<PagedGamesResponse> GetMostAnticipatedAsync(int page, int pageSize, CancellationToken ct);
    Task<PagedGamesResponse> GetPopscoreAsync(
        long popularityTypeId,
        DateTime? from,
        DateTime? to,
        int page,
        int pageSize,
        CancellationToken ct
    );
    Task<StatisticsResponse> GetStatisticsAsync(CancellationToken ct);
}
