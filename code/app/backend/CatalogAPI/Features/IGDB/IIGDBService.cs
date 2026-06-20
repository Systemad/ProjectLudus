using CatalogAPI.Features.Games.Common.Dtos;
using CatalogAPI.Features.IGDB.GetPopscore;
using CatalogAPI.Features.IGDB.GetStatistics;

namespace CatalogAPI.Features.IGDB;

public interface IIGDBService
{
    Task<List<GameBrowseDto>> GetMostAnticipatedAsync(int? limit, CancellationToken ct);
    Task<GetPopscoreResponse> GetPopscoreAsync(long popularityTypeId, int limit, CancellationToken ct);
    Task<StatisticsResponse> GetStatisticsAsync(CancellationToken ct);
}
