using Shared.Features;
using Shared.Features.Games;

namespace WebAPI.Features.Common.Games.Mappers;

public static partial class GameMappingExtensions
{
    public static IgdbGameDto MapToIgdbGameDto(
        this IGDBGameFlat igdbGameFlat,
        List<GameEngine> gameEngines,
        List<GameMode> gameModes,
        List<Genre> genres,
        List<InvolvedCompany> involvedCompanies,
        List<Keyword> keywords,
        List<Platform> platforms,
        List<PlayerPerspective> playerPerspectives,
        List<Theme> themes)
    {
        return new IgdbGameDto
        {
            Id = igdbGameFlat.Id,
            AgeRatings = igdbGameFlat.AgeRatings,
            AlternativeNames = igdbGameFlat.AlternativeNames,
            Artworks = igdbGameFlat.Artworks,
            Cover = igdbGameFlat.Cover,
            CreatedAt = igdbGameFlat.CreatedAt,
            Dlcs = igdbGameFlat.Dlcs,
            Expansions = igdbGameFlat.Expansions,
            FirstReleaseDate = igdbGameFlat.FirstReleaseDate,
            Franchises = igdbGameFlat.Franchises,
            GameEngines = gameEngines,
            GameModes = gameModes,
            Genres = genres,
            Hypes = igdbGameFlat.Hypes,
            InvolvedCompanies = involvedCompanies,
            Keywords = keywords,
            Name = igdbGameFlat.Name,
            Platforms = platforms,
            PlayerPerspectives = playerPerspectives,
            Rating = igdbGameFlat.Rating,
            RatingCount = igdbGameFlat.RatingCount,
            ReleaseDates = igdbGameFlat.ReleaseDates,
            Screenshots = igdbGameFlat.Screenshots,
            SimilarGames = igdbGameFlat.SimilarGames,
            Slug = igdbGameFlat.Slug,
            Storyline = igdbGameFlat.Storyline,
            Summary = igdbGameFlat.Summary,
            Themes = themes,
            TotalRating = igdbGameFlat.TotalRating,
            TotalRatingCount = igdbGameFlat.TotalRatingCount,
            UpdatedAt = igdbGameFlat.UpdatedAt,
            Url = igdbGameFlat.Url,
            Videos = igdbGameFlat.Videos,
            Websites = igdbGameFlat.Websites,
            Checksum = igdbGameFlat.Checksum,
            LanguageSupports = igdbGameFlat.LanguageSupports,
            Collections = igdbGameFlat.Collections,
            GameType = igdbGameFlat.GameType,
            MultiplayerModes = igdbGameFlat.MultiplayerModes
        };
    }
}