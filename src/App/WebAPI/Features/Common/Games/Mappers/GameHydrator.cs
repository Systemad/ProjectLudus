using Shared.Features;

namespace WebAPI.Features.Common.Games.Mappers;

public class GameHydrator
{
    private HydrationCache _hydrationCache { get; set; }

    public GameHydrator(HydrationCache hydrationCache)
    {
        _hydrationCache = hydrationCache;
    }

    public IGDBGame HydrateGameAsync(InsertIGDBGame game)
    {
        return new IGDBGame
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
            GameEngines = _hydrationCache.GetGameEngines(game.GameEngines),
            GameModes = _hydrationCache.GetGameModes(game.GameModes),
            Genres = _hydrationCache.GetGenres(game.Genres),
            Hypes = game.Hypes,
            InvolvedCompanies = game.InvolvedCompanies,
            Keywords = _hydrationCache.GetKeywords(game.Keywords),
            Name = game.Name,
            Platforms = _hydrationCache.GetPlatforms(game.Platforms),
            PlayerPerspectives = _hydrationCache.GetPlayerPerspectives(game.PlayerPerspectives),
            Rating = game.Rating,
            RatingCount = game.RatingCount,
            ReleaseDates = game.ReleaseDates,
            Screenshots = game.Screenshots,
            SimilarGames = game.SimilarGames,
            Slug = game.Slug,
            Storyline = game.Storyline,
            Summary = game.Summary,
            Themes = _hydrationCache.GetPlayerThemes(game.Themes),
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