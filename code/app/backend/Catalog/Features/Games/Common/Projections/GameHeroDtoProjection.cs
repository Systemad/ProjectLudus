using Catalog.Features.Games.Browse.GetHero;
using Catalog.Features.Games.Common.Dtos;
using Data.Models;

namespace Catalog.Features.Games.Common.Projections;

internal static class GameHeroDtoProjection
{
    public static IQueryable<GameHeroDto> SelectGameHeroDto(this IQueryable<Game> games) =>
        games.Select(g => new GameHeroDto
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
        });
}
