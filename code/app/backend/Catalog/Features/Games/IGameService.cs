using Catalog.Features.Games.Browse.Get;
using Catalog.Features.Games.Browse.GetByFilter;
using Catalog.Features.Games.Browse.GetHero;
using Catalog.Features.Games.Browse.GetMedia;
using Catalog.Features.Games.Browse.GetOverview;
using Catalog.Features.Games.Common.Dtos;
using Catalog.Features.Games.Common.Pagination;
using Catalog.Features.Games.Page.GetReleaseData;

namespace Catalog.Features.Games;

public interface IGameService
{
    Task<GameOverviewDto?> GetOverviewAsync(long gameId, CancellationToken ct);
    Task<GameDetailsDto?> GetDetailsAsync(long gameId, CancellationToken ct);
    Task<GameHeroDto?> GetHeroAsync(long gameId, CancellationToken ct);
    Task<List<GameHeroDto>> GetHeroesAsync(IReadOnlyCollection<long> gameIds, CancellationToken ct);
    Task<GameMediaDto?> GetMediaAsync(long gameId, CancellationToken ct);
    Task<GamePageReleaseDataDto?> GetReleaseDataAsync(long gameId, CancellationToken ct);
    Task<List<WebsiteDto>> GetLinksAsync(long gameId, CancellationToken ct);
    Task<List<GameBrowseDto>> GetSimilarGamesAsync(long gameId, CancellationToken ct);
    Task<List<GameBrowseDto>> GetByReleaseDateRangeAsync(
        DateTime start,
        DateTime end,
        int limit,
        CancellationToken ct
    );
    Task<PagedGamesResponse> GetGamesByFilterAsync(Request request, CancellationToken ct);
}
