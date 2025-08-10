using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Common.Games.Models;

namespace WebAPI.Features.Common.Games.Mappers;

public static class GameMapper
{
    public static GameDto MapToGameDto(this InsertIGDBGame game, List<Platform> platforms, bool isWishlisted,
        bool isHyped)
    {
        return new GameDto()
        {
            Id = game.Id,
            Name = game.Name,
            ArtworkImageId = game.Artworks?.FirstOrDefault()?.ImageId ?? "",
            CoverImageId = game.Cover?.ImageId ?? "",
            FirstReleaseDate = game.FirstReleaseDate,
            //InvolvedCompanies = await _hydrationCache.game.InvolvedCompanies
            Platforms = platforms,
            ReleaseDates =
                game.ReleaseDates?.Select(rd => DateTimeOffset.FromUnixTimeSeconds(rd.Date).DateTime).ToList() ??
                [],
            GameType = game.GameType,
            IsWishlisted = isWishlisted,
            IsHyped = isHyped,
        };
    }

    public static IGDBGame MapToGame(this InsertIGDBGame game, List<GameEngine> gameEngines, List<GameMode> gameModes,
        List<Genre> genres, List<InvolvedCompany> involvedCompanies, List<Keyword> keywords, List<Platform> platforms,
        List<PlayerPerspective> playerPerspectives, List<Theme> themes)
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
            GameEngines = gameEngines,
            GameModes = gameModes,
            Genres = genres,
            Hypes = game.Hypes,
            InvolvedCompanies = involvedCompanies,
            Keywords = keywords,
            Name = game.Name,
            Platforms = platforms,
            PlayerPerspectives = playerPerspectives,
            Rating = game.Rating,
            RatingCount = game.RatingCount,
            ReleaseDates = game.ReleaseDates,
            Screenshots = game.Screenshots,
            SimilarGames = game.SimilarGames,
            Slug = game.Slug,
            Storyline = game.Storyline,
            Summary = game.Summary,
            Themes = themes,
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