using CatalogAPI.Features.Games.Browse.Get;
using CatalogAPI.Features.Games.Browse.GetHero;
using CatalogAPI.Features.Games.Browse.GetMedia;
using CatalogAPI.Features.Games.Browse.GetOverview;
using CatalogAPI.Features.Games.Page.GetReleaseData;
using Data;

namespace CatalogAPI.Features.Games;

internal sealed class GameService(AppDbContext db) : IGameService
{
    public async Task<GameOverviewDto?> GetOverviewAsync(
        long gameId, CancellationToken ct)
    {
        return await db.Games.Where(g => g.Id == gameId)
            .AsSplitQuery()
            .Select(g => new GameOverviewDto
            {
                Id = g.Id, Slug = g.Slug, Name = g.Name, Summary = g.Summary, Storyline = g.Storyline,
                Cover = g.CoverNavigation!.ImageId, CoverUrl = g.CoverNavigation!.Url,
                GameType = g.GameType, GameTypeName = g.GameTypeNavigation!.Type,
                Genres = g.Genres.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name!).ToList(),
                Themes = g.Themes.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name!).ToList(),
                Platforms = g.Platforms.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(p => new PlatformsDto(p.Name!, p.Slug)).ToList(),
                IsReleased = g.FirstReleaseDateUtc <= DateTime.UtcNow,
                ReleaseDatePlatform = g.ReleaseDates
                    .Select(rd => new ReleaseDatePlatformDto(rd.Date, rd.PlatformNavigation!.Name)).ToList(),
                ReleaseDates = g.ReleaseDates
                    .Select(rd => new ReleaseDatesDto(rd.Date, rd.ReleaseRegionNavigation!.Region)).ToList(),
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<GameDetailsDto?> GetDetailsAsync(
        long gameId, CancellationToken ct)
    {
        return await db.Games.Where(g => g.Id == gameId)
            .AsSplitQuery()
            .Select(g => new GameDetailsDto
            {
                Id = g.Id, Url = g.Url,
                InvolvedCompanies = g.InvolvedCompanies.Select(ic => new InvolvedCompanyDto(
                    ic.Id,
                    ic.Company,
                    ic.CompanyNavigation.Name,
                    ic.CompanyNavigation.Slug,
                    ic.Developer,
                    ic.Publisher,
                    ic.Porting,
                    ic.Supporting
                )).ToList(),
                Themes = g.Themes.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name!).ToList(),
                GameModes = g.GameModes.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name!).ToList(),
                PlayerPerspectives = g.PlayerPerspectives.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name!).ToList(),
                Websites = g.Websites.Where(w => !string.IsNullOrEmpty(w.Url))
                    .Select(w => new WebsiteDto(w.Url!, w.TypeNavigation!.Type, w.Url, w.Trusted)).ToList(),
                AlternativeNames = g.AlternativeNames
                    .Select(a => new AlternativeNameDto(a.Id, a.Name, a.Comment)).ToList(),
                GameEngines = g.GameEngines
                    .Select(ge => new GameEnginesDto(ge.Id, ge.Name, ge.LogoNavigation!.ImageId, ge.Url)).ToList(),
                LanguageSupports = g.GameLocalizations.Where(l => !string.IsNullOrEmpty(l.Name))
                    .Select(l => new LanguageSupportsDto(l.Name!, null, l.RegionNavigation!.Name)).ToList(),
                Franchises = g.Franchises.Where(f => !string.IsNullOrEmpty(f.Name) && !string.IsNullOrEmpty(f.Slug))
                    .Select(f => new FranchiseDto(f.Name!, f.Slug!)).ToList(),
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<GameHeroDto?> GetHeroAsync(
        long gameId, CancellationToken ct)
    {
        return await db.Games.Where(g => g.Id == gameId)
            .AsSplitQuery()
            .Select(g => new GameHeroDto
            {
                Id = g.Id, Slug = g.Slug, Name = g.Name, Summary = g.Summary,
                Cover = g.CoverNavigation!.ImageId, CoverUrl = g.CoverNavigation!.Url,
                GameTypeName = g.GameTypeNavigation!.Type,
                FirstReleaseDate = g.FirstReleaseDateUtc != null ? DateOnly.FromDateTime(g.FirstReleaseDateUtc.Value) : null,
                Genres = g.Genres.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!)).ToList(),
                Themes = g.Themes.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!)).ToList(),
                GameModes = g.GameModes.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!)).ToList(),
                Keywords = g.Keywords.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!)).ToList(),
                PlayerPerspectives = g.PlayerPerspectives.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new Feature(x.Name!, x.Slug!)).ToList(),
                Platforms = g.Platforms.Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(p => new PlatformDto(p.Id, p.Name, p.Slug)).ToList(),
                Companies = g.InvolvedCompanies
                    .Select(ic => new InvolvedCompanyDto(
                        ic.Id, ic.Company, ic.CompanyNavigation.Name, ic.CompanyNavigation.Slug,
                        ic.Developer, ic.Publisher, ic.Porting, ic.Supporting)).ToList(),
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<GameMediaDto?> GetMediaAsync(
        long gameId, CancellationToken ct)
    {
        return await db.Games.Where(g => g.Id == gameId)
            .Select(g => new GameMediaDto
            {
                Screenshots = g.Screenshots.Where(s => !string.IsNullOrEmpty(s.ImageId))
                    .Select(s => s.ImageId!).ToList(),
                Videos = g.Videos
                    .Select(v => new GameMediaVideoDto(v.Name, v.VideoId ?? string.Empty)).ToList(),
            })
            .SingleOrDefaultAsync(ct);
    }

    public async Task<GamePageReleaseDataDto?> GetReleaseDataAsync(
        long gameId, CancellationToken ct)
    {
        return await db.Games.Where(g => g.Id == gameId)
            .AsSplitQuery()
            .Select(g => new GamePageReleaseDataDto
            {
                Id = g.Id, Name = g.Name, Slug = g.Slug,
                Releases = g.ReleaseDates.Where(rd => rd.PlatformNavigation != null)
                    .Select(rd => new GameReleaseDto
                    {
                        PlatformName = rd.PlatformNavigation!.Name, PlatformSlug = rd.PlatformNavigation!.Slug,
                        ReleaseDate = rd.Date, Region = rd.ReleaseRegionNavigation!.Region, Human = rd.Human,
                        Status = rd.StatusNavigation != null
                            ? new ReleaseDateStatusDto(rd.StatusNavigation.Id, rd.StatusNavigation.Name!) : null,
                        Platform = rd.PlatformNavigation != null
                            ? new PlatformDto(rd.PlatformNavigation.Id, rd.PlatformNavigation.Name, rd.PlatformNavigation.Slug) : null,
                        InvolvedCompanies = g.InvolvedCompanies
                            .Select(ic => new InvolvedCompanyDto(
                                ic.Id, ic.Company, ic.CompanyNavigation.Name, ic.CompanyNavigation.Slug,
                                ic.Developer, ic.Publisher, ic.Porting, ic.Supporting)).ToList(),
                    }).ToList(),
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<WebsiteDto>> GetLinksAsync(
        long gameId, CancellationToken ct)
    {
        return await db.Games.Where(g => g.Id == gameId)
            .SelectMany(g => g.Websites)
            .Where(w => !string.IsNullOrEmpty(w.Url))
            .Select(w => new WebsiteDto(w.Url!, w.TypeNavigation!.Type, w.Url!, w.Trusted))
            .ToListAsync(ct);
    }

    public async Task<List<GameBrowseDto>> GetSimilarGamesAsync(
        long gameId, CancellationToken ct)
    {
        return await db.Games.Where(g => g.Id == gameId)
            .SelectMany(g => g.SimilarGames)
            .Where(rg => rg.FirstReleaseDateUtc.HasValue)
            .SelectGameBrowseDto()
            .ToListAsync(ct);
    }

    public async Task<List<GameBrowseDto>> GetByReleaseDateRangeAsync(
        DateTime start, DateTime end, int limit, CancellationToken ct)
    {
        return await db.Games
            .OrderBy(g => g.FirstReleaseDateUtc)
            .ThenByDescending(g => g.AggregatedRating)
            .Take(limit)
            .SelectGameBrowseDto()
            .ToListAsync(ct);
    }
}
