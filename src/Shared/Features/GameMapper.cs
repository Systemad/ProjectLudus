using Shared.Features.Games;

namespace Shared.Features;

public static class GameMapper
{
    public static List<InsertIgdbGame> FlattenGames(this List<IgdbGame> game)
    {
        return new List<InsertIgdbGame>(game.Select(x => x.FlattenGameEntity()));
    }
    
    public static InsertIgdbGame FlattenGameEntity(this IgdbGame game)
    {
        return new InsertIgdbGame
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