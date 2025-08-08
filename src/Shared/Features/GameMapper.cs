using Shared.Features.Games;

namespace Shared.Features;

public static class GameMapper
{
    public static List<InsertIGDBGame> FlattenGames(this List<IGDBGame> game)
    {
        return new List<InsertIGDBGame>(game.Select(x => x.FlattenGameEntity()));
    }
    
    public static InsertIGDBGame FlattenGameEntity(this IGDBGame game)
    {
        return new InsertIGDBGame
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
            GameEngines = game.GameEngines?.Select(x => x.Id).ToList() ?? new List<long>(),
            GameModes = game.GameModes?.Select(x => x.Id).ToList() ?? new List<long>(),
            Genres = game.Genres?.Select(x => x.Id).ToList() ?? new List<long>(),
            Hypes = game.Hypes,
            InvolvedCompanies = game.InvolvedCompanies?
                .Select(ic => new InvolvedCompanyEntity
                {
                    Id = ic.Id,
                    Company = ic.Company.Id,
                    Developer = ic.Developer,
                    Porting = ic.Porting,
                    Publisher = ic.Publisher,
                    Supporting = ic.Supporting
                })
                .ToList() ?? new List<InvolvedCompanyEntity>(),
            Keywords = game.Keywords?.Select(x => x.Id).ToList() ?? new List<long>(),
            Name = game.Name,
            Platforms = game.Platforms?.Select(x => x.Id).ToList() ?? new List<long>(),
            PlayerPerspectives = game.PlayerPerspectives?.Select(x => x.Id).ToList() ?? new List<long>(),
            Rating = game.Rating,
            RatingCount = game.RatingCount,
            ReleaseDates = game.ReleaseDates,
            Screenshots = game.Screenshots,
            SimilarGames = game.SimilarGames,
            Slug = game.Slug,
            Storyline = game.Storyline,
            Summary = game.Summary,
            Themes = game.Themes?.Select(x => x.Id).ToList() ?? new List<long>(),
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