using Shared.Features.Games;

namespace Shared.Features;

public static class NormalizedGameMapper
{
    public static List<IGDBGameFlat> NormalizeGames(this List<IGDBGameRaw> game)
    {
        return new List<IGDBGameFlat>(game.Select(x => x.NormalizeGame()));
    }
    
    public static IGDBGameFlat NormalizeGame(this IGDBGameRaw game)
    {
        return new IGDBGameFlat
        {
            Id = game.Id,
            AgeRatings = game.AgeRatings,
            AlternativeNames = game.AlternativeNames,
            Artworks = game.Artworks,
            Cover = game.Cover,
            CreatedAt = game.CreatedAt,
            Dlcs = game.Dlcs,
            Expansions = game.Expansions,
            FirstReleaseDate = game.FirstReleaseDate,
            Franchises = game.Franchises,
            GameEngines = game.GameEngines.Select(x => x.Id).ToArray(),
            GameModes = game.GameModes.Select(x => x.Id).ToArray(),
            Genres = game.Genres.Select(x => x.Id).ToArray(),
            Hypes = game.Hypes,
            InvolvedCompanies = game.InvolvedCompanies
                .Select(x => new InvolvedCompanyFlat
                {
                    Id = x.Id,
                    CompanyId = x.Company.Id,
                    Developer = x.Developer,
                    Porting = x.Porting,
                    Publisher = x.Publisher,
                    Supporting = x.Supporting
                })
                .ToList(),
            Keywords = game.Keywords.Select(x => x.Id).ToArray(),
            Name = game.Name,
            Platforms = game.Platforms.Select(x => x.Id).ToArray(),
            PlayerPerspectives = game.PlayerPerspectives.Select(x => x.Id).ToArray(),
            Rating = game.Rating,
            RatingCount = game.RatingCount,
            ReleaseDates = game.ReleaseDates,
            Screenshots = game.Screenshots,
            SimilarGames = game.SimilarGames,
            Slug = game.Slug,
            Storyline = game.Storyline,
            Summary = game.Summary,
            Themes = game.Themes.Select(x => x.Id).ToArray(),
            TotalRating = game.TotalRating,
            TotalRatingCount = game.TotalRatingCount,
            UpdatedAt = game.UpdatedAt,
            Url = game.Url,
            Videos = game.Videos,
            Websites = game.Websites,
            Checksum = game.Checksum,
            LanguageSupports = game.LanguageSupports,
            Collections = game.Collections,
            GameType = game.GameType,
            MultiplayerModes = game.MultiplayerModes
        };
    }
}