using CatalogAPI.Features.Games.Common.Dtos;
using CatalogAPI.Features.Homepage.GetPopularityTables;

namespace CatalogAPI.Features.Homepage;

public interface IHomepageService
{
    Task<List<GameBrowseDto>> GetUpcomingAsync(CancellationToken ct);
    Task<PopularityTablesResponse> GetPopularityTablesAsync(CancellationToken ct);
}
