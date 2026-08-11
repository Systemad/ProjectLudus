using Catalog.Features.Games.Common.Dtos;
using Data.Models;

namespace Catalog.Features.Games.Common.Projections;

public static class GameBrowseDtoProjection
{
    public static IQueryable<GameBrowseDto> SelectGameBrowseDto(this IQueryable<Game> query) =>
        query.Select(g => new GameBrowseDto
        {
            Id = g.Id.ToString(),
            Name = g.Name,
            FirstReleaseDate =
                g.FirstReleaseDateUtc != null
                    ? DateOnly.FromDateTime(g.FirstReleaseDateUtc.Value)
                    : null,
            CoverUrl = g.CoverNavigation!.ImageId,
            Steam = new SteamData(
                g.SteamLatestPlayerCount!.SteamAppId.HasValue
                    ? g.SteamLatestPlayerCount!.SteamAppId.Value.ToString()
                    : null,
                g.SteamLatestPlayerCount!.CurrentPlayers,
                g.SteamLatestPlayerCount!.Peak24h,
                g.SteamDetail!.HeaderUrl,
                g.SteamDetail!.CapsuleUrl
            ),
            Pricing = new SteamPricingData(
                g.SteamLatestPricing!.FinalCents,
                g.SteamLatestPricing!.DiscountPercent,
                g.SteamLatestPricing!.Currency,
                g.SteamLatestPricing!.InitialCents,
                g.SteamLatestPricing!.InitialFormatted,
                g.SteamLatestPricing!.FinalFormatted,
                g.SteamLatestPricing!.High30d,
                g.SteamLatestPricing!.Low30d
            ),
            Review = new SteamReviewData(
                g.SteamReview!.ReviewScore,
                g.SteamReview!.ReviewScoreDesc,
                g.SteamReview!.TotalReviews,
                g.SteamReview!.TotalPositive,
                g.SteamReview!.TotalNegative
            ),
            GameFeatures = new GameFeatures(
                g.Genres.Select(t => new Feature(t.Name!, t.Slug!)).ToList(),
                g.Themes.Select(t => new Feature(t.Name!, t.Slug!)).ToList()
            ),
            Platforms = g.Platforms
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
