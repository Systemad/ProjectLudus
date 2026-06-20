using CatalogAPI.Features.Games.Browse.Get;
using CatalogAPI.Features.Games.Browse.GetHero;
using CatalogAPI.Features.Games.Browse.GetMedia;
using CatalogAPI.Features.Games.Browse.GetOverview;
using CatalogAPI.Features.Games.Page.GetReleaseData;

namespace CatalogAPI.Features.Games;

public interface IGameService
{
    Task<GameOverviewDto?> GetOverviewAsync(long gameId, CancellationToken ct);
    Task<GameDetailsDto?> GetDetailsAsync(long gameId, CancellationToken ct);
    Task<GameHeroDto?> GetHeroAsync(long gameId, CancellationToken ct);
    Task<GameMediaDto?> GetMediaAsync(long gameId, CancellationToken ct);
    Task<GamePageReleaseDataDto?> GetReleaseDataAsync(long gameId, CancellationToken ct);
    Task<List<WebsiteDto>> GetLinksAsync(long gameId, CancellationToken ct);
    Task<List<GameBrowseDto>> GetSimilarGamesAsync(long gameId, CancellationToken ct);
    Task<List<GameBrowseDto>> GetByReleaseDateRangeAsync(DateTime start, DateTime end, int limit, CancellationToken ct);
}
