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
            Steam =
                g.SteamDetail == null || !g.SteamDetail.SteamAppId.HasValue
                    ? null
                    : new SteamData
                    {
                        SteamAppId = g.SteamDetail.SteamAppId.Value.ToString(),
                        CurrentPlayers =
                            g.SteamLatestPlayerCount != null
                                ? g.SteamLatestPlayerCount.CurrentPlayers
                                : null,
                        Peak24h =
                            g.SteamLatestPlayerCount != null
                                ? g.SteamLatestPlayerCount.Peak24h
                                : null,
                        HeaderUrl = g.SteamDetail.HeaderUrl,
                        CapsuleUrl = g.SteamDetail.CapsuleUrl,
                        Pricing =
                            g.SteamLatestPricing == null
                                ? null
                                : new SteamPricingData
                                {
                                    FinalCents = g.SteamLatestPricing.FinalCents,
                                    DiscountPercent = g.SteamLatestPricing.DiscountPercent,
                                    Currency = g.SteamLatestPricing.Currency,
                                    InitialCents = g.SteamLatestPricing.InitialCents,
                                    InitialFormatted = g.SteamLatestPricing.InitialFormatted,
                                    FinalFormatted = g.SteamLatestPricing.FinalFormatted,
                                    High30d = g.SteamLatestPricing.High30d,
                                    Low30d = g.SteamLatestPricing.Low30d,
                                },
                        Review =
                            g.SteamReview == null
                                ? null
                                : new SteamReviewData
                                {
                                    Score = g.SteamReview.ReviewScore,
                                    Desc = g.SteamReview.ReviewScoreDesc,
                                    TotalReviews = g.SteamReview.TotalReviews,
                                    Positive = g.SteamReview.TotalPositive,
                                    Negative = g.SteamReview.TotalNegative,
                                },
                    },
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
