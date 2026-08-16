using Catalog.Features.Games.Browse.Get;
using Catalog.Features.Games.Browse.GetByFilter;
using Catalog.Features.Games.Browse.GetHero;
using Catalog.Features.Games.Browse.GetMedia;
using Catalog.Features.Games.Browse.GetOverview;
using Catalog.Features.Games.Common.Dtos;
using Catalog.Features.Games.Common.Pagination;
using Catalog.Features.Games.Common.Projections;
using Catalog.Features.Games.Page.GetReleaseData;
using Data.Models;

namespace Catalog.Features.Games;

internal sealed class GameService(AppDbContext db) : IGameService
{
    public async Task<GameOverviewDto?> GetOverviewAsync(long gameId, CancellationToken ct)
    {
        return await db
            .Games.Where(g => g.Id == gameId)
            .AsSplitQuery()
            .Select(g => new GameOverviewDto
            {
                Id = g.Id.ToString(),
                Slug = g.Slug,
                Name = g.Name,
                Summary = g.Summary,
                Storyline = g.Storyline,
                Cover = g.CoverNavigation!.ImageId,
                CoverUrl = g.CoverNavigation!.Url,
                GameType = g.GameType.HasValue ? g.GameType.Value.ToString() : null,
                GameTypeName = g.GameTypeNavigation!.Type,
                Steam =
                    g.SteamLatestPlayerCount == null
                        ? new SteamData(null, null, null, null, null)
                        : new SteamData(
                            g.SteamLatestPlayerCount.SteamAppId.HasValue
                                ? g.SteamLatestPlayerCount.SteamAppId.Value.ToString()
                                : null,
                            g.SteamLatestPlayerCount.CurrentPlayers,
                            g.SteamLatestPlayerCount.Peak24h,
                            null,
                            null
                        ),
                Genres = g
                    .Genres.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => x.Name!)
                    .ToList(),
                Themes = g
                    .Themes.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => x.Name!)
                    .ToList(),
                Platforms = g
                    .Platforms.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(p => new PlatformsDto(p.Name!, p.Slug))
                    .ToList(),
                IsReleased = g.FirstReleaseDateUtc <= DateTime.UtcNow,
                ReleaseDatePlatform = g
                    .ReleaseDates.Select(rd => new ReleaseDatePlatformDto(
                        rd.Date,
                        rd.PlatformNavigation!.Name
                    ))
                    .ToList(),
                ReleaseDates = g
                    .ReleaseDates.Select(rd => new ReleaseDatesDto(
                        rd.Date,
                        rd.ReleaseRegionNavigation!.Region
                    ))
                    .ToList(),
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<GameDetailsDto?> GetDetailsAsync(long gameId, CancellationToken ct)
    {
        return await db
            .Games.Where(g => g.Id == gameId)
            .AsSplitQuery()
            .Select(g => new GameDetailsDto
            {
                Id = g.Id.ToString(),
                Url = g.Url,
                InvolvedCompanies = g
                    .InvolvedCompanies.Select(ic => new InvolvedCompanyDto(
                        ic.Id.ToString(),
                        ic.Company.ToString(),
                        ic.CompanyNavigation.Name,
                        ic.CompanyNavigation.Slug,
                        (
                            ic.CompanyNavigation.LogoNavigation != null
                                ? ic.CompanyNavigation.LogoNavigation.ImageId
                                : null
                        ) ?? string.Empty,
                        ic.Developer,
                        ic.Publisher,
                        ic.Porting,
                        ic.Supporting
                    ))
                    .ToList(),
                Themes = g
                    .Themes.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => x.Name!)
                    .ToList(),
                GameModes = g
                    .GameModes.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => x.Name!)
                    .ToList(),
                PlayerPerspectives = g
                    .PlayerPerspectives.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => x.Name!)
                    .ToList(),
                Websites = g
                    .Websites.Where(w => !string.IsNullOrEmpty(w.Url))
                    .Select(w => new WebsiteDto(w.Url!, w.TypeNavigation!.Type, w.Url, w.Trusted))
                    .ToList(),
                AlternativeNames = g
                    .AlternativeNames.Select(a =>
                        new AlternativeNameDto(a.Id.ToString(), a.Name, a.Comment)
                    )
                    .ToList(),
                GameEngines = g
                    .GameEngines.Select(ge => new GameEnginesDto(
                        ge.Id.ToString(),
                        ge.Name,
                        ge.LogoNavigation!.ImageId,
                        ge.Url
                    ))
                    .ToList(),
                LanguageSupports = g
                    .GameLocalizations.Where(l => !string.IsNullOrEmpty(l.Name))
                    .Select(l => new LanguageSupportsDto(l.Name!, null, l.RegionNavigation!.Name))
                    .ToList(),
                Franchises = g
                    .Franchises.Where(f =>
                        !string.IsNullOrEmpty(f.Name) && !string.IsNullOrEmpty(f.Slug)
                    )
                    .Select(f => new FranchiseDto(f.Name!, f.Slug!))
                    .ToList(),
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<GameHeroDto?> GetHeroAsync(long gameId, CancellationToken ct)
    {
        return await db
            .Games.Where(g => g.Id == gameId)
            .AsSplitQuery()
            .Select(g => new GameHeroDto
            {
                Id = g.Id.ToString(),
                Slug = g.Slug,
                Name = g.Name,
                Summary = g.Summary,
                Cover = g.CoverNavigation!.ImageId,
                CoverUrl = g.CoverNavigation!.Url,
                GameTypeName = g.GameTypeNavigation!.Type,
                FirstReleaseDate =
                    g.FirstReleaseDateUtc != null
                        ? DateOnly.FromDateTime(g.FirstReleaseDateUtc.Value)
                        : null,
                Genres = g
                    .Genres.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!))
                    .ToList(),
                Themes = g
                    .Themes.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!))
                    .ToList(),
                GameModes = g
                    .GameModes.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!))
                    .ToList(),
                Keywords = g
                    .Keywords.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!))
                    .ToList(),
                PlayerPerspectives = g
                    .PlayerPerspectives.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!))
                    .ToList(),
                Platforms = g
                    .Platforms.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(p => new PlatformDto(p.Id.ToString(), p.Name, p.Slug))
                    .ToList(),
                Companies = g
                    .InvolvedCompanies.Select(ic => new InvolvedCompanyDto(
                        ic.Id.ToString(),
                        ic.Company.ToString(),
                        ic.CompanyNavigation.Name,
                        ic.CompanyNavigation.Slug,
                        (
                            ic.CompanyNavigation.LogoNavigation != null
                                ? ic.CompanyNavigation.LogoNavigation.ImageId
                                : null
                        ) ?? string.Empty,
                        ic.Developer,
                        ic.Publisher,
                        ic.Porting,
                        ic.Supporting
                    ))
                    .ToList(),
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<GameMediaDto?> GetMediaAsync(long gameId, CancellationToken ct)
    {
        return await db
            .Games.Where(g => g.Id == gameId)
            .Select(g => new GameMediaDto
            {
                Screenshots = g
                    .Screenshots.Where(s => !string.IsNullOrEmpty(s.ImageId))
                    .Select(s => s.ImageId!)
                    .ToList(),
                Videos = g
                    .Videos.Select(v => new GameMediaVideoDto(v.Name, v.VideoId ?? string.Empty))
                    .ToList(),
            })
            .SingleOrDefaultAsync(ct);
    }

    public async Task<GamePageReleaseDataDto?> GetReleaseDataAsync(
        long gameId,
        CancellationToken ct
    )
    {
        return await db
            .Games.Where(g => g.Id == gameId)
            .AsSplitQuery()
            .Select(g => new GamePageReleaseDataDto
            {
                Id = g.Id.ToString(),
                Name = g.Name,
                Slug = g.Slug,
                Releases = g
                    .ReleaseDates.Where(rd => rd.PlatformNavigation != null)
                    .Select(rd => new GameReleaseDto
                    {
                        PlatformName = rd.PlatformNavigation!.Name,
                        PlatformSlug = rd.PlatformNavigation!.Slug,
                        ReleaseDate = rd.Date,
                        Region = rd.ReleaseRegionNavigation!.Region,
                        Human = rd.Human,
                        Status =
                            rd.StatusNavigation != null
                                ? new ReleaseDateStatusDto(
                                    rd.StatusNavigation.Id.ToString(),
                                    rd.StatusNavigation.Name!
                                )
                                : null,
                        Platform =
                            rd.PlatformNavigation != null
                                ? new PlatformDto(
                                    rd.PlatformNavigation.Id.ToString(),
                                    rd.PlatformNavigation.Name,
                                    rd.PlatformNavigation.Slug
                                )
                                : null,
                        InvolvedCompanies = g
                            .InvolvedCompanies.Select(ic => new InvolvedCompanyDto(
                                ic.Id.ToString(),
                                ic.Company.ToString(),
                                ic.CompanyNavigation.Name,
                                ic.CompanyNavigation.Slug,
                                (
                                    ic.CompanyNavigation.LogoNavigation != null
                                        ? ic.CompanyNavigation.LogoNavigation.ImageId
                                        : null
                                ) ?? string.Empty,
                                ic.Developer,
                                ic.Publisher,
                                ic.Porting,
                                ic.Supporting
                            ))
                            .ToList(),
                    })
                    .ToList(),
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<WebsiteDto>> GetLinksAsync(long gameId, CancellationToken ct)
    {
        return await db
            .Games.Where(g => g.Id == gameId)
            .SelectMany(g => g.Websites)
            .Where(w => !string.IsNullOrEmpty(w.Url))
            .Select(w => new WebsiteDto(w.Url!, w.TypeNavigation!.Type, w.Url!, w.Trusted))
            .ToListAsync(ct);
    }

    public async Task<List<GameBrowseDto>> GetSimilarGamesAsync(long gameId, CancellationToken ct)
    {
        return await db
            .Games.Where(g => g.Id == gameId)
            .SelectMany(g => g.SimilarGames)
            .Where(rg => rg.FirstReleaseDateUtc.HasValue)
            .SelectGameBrowseDto()
            .ToListAsync(ct);
    }

    public async Task<List<GameBrowseDto>> GetByReleaseDateRangeAsync(
        DateTime start,
        DateTime end,
        int limit,
        CancellationToken ct
    )
    {
        return await db
            .Games.OrderBy(g => g.FirstReleaseDateUtc)
            .ThenByDescending(g => g.AggregatedRating)
            .Take(limit)
            .SelectGameBrowseDto()
            .ToListAsync(ct);
    }

    public async Task<PagedGamesResponse> GetGamesByFilterAsync(
        Request request,
        CancellationToken ct
    )
    {
        IQueryable<Game> query = db.Games;

        if (!string.IsNullOrWhiteSpace(request.Genre))
        {
            query = query.Where(game => game.Genres.Any(genre => genre.Slug == request.Genre));
        }

        if (!string.IsNullOrWhiteSpace(request.Theme))
        {
            query = query.Where(game => game.Themes.Any(theme => theme.Slug == request.Theme));
        }

        if (!string.IsNullOrWhiteSpace(request.GameMode))
        {
            query = query.Where(game => game.GameModes.Any(mode => mode.Slug == request.GameMode));
        }

        if (request.From.HasValue)
        {
            query = query.Where(game => game.FirstReleaseDateUtc >= request.From.Value);
        }

        if (request.To.HasValue)
        {
            query = query.Where(game => game.FirstReleaseDateUtc < request.To.Value);
        }

        var page = request.PageNumber;
        var pageSize = request.Size;
        var skip = (page - 1) * pageSize;

        var games = await query
            .OrderByDescending(game => game.Hypes)
            .ThenByDescending(game => game.TotalRatingCount)
            .ThenByDescending(game => game.TotalRating)
            .ThenBy(game => game.Id)
            .Skip(skip)
            .Take(pageSize + 1)
            .SelectGameBrowseDto()
            .ToListAsync(ct);

        return PagedGamesResponse.Create(games, page, pageSize);
    }

}
