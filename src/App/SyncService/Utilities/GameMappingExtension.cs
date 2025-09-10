using NodaTime;
using Shared.Features;

namespace SyncService.Utilities;

public static class GameMappingExtension
{
    public static GameEntity ToEntity(this IGDBGame game)
    {
        return new GameEntity()
        {
            Id = game.Id,
            Name = game.Name,
            ReleaseDate = Instant.FromUnixTimeSeconds(game.FirstReleaseDate),
            GameType = game.GameType.Id,
            Platforms = game.Platforms?.Select(x => x.Id).ToArray() ?? [],
            GameEngines = game.GameEngines?.Select(x => x.Id).ToArray() ?? [],
            Genres = game.Genres?.Select(x => x.Id).ToArray() ?? [],
            Themes = game.Themes?.Select(x => x.Id).ToArray() ?? [],
            Rating = game.Rating ?? 0,
            RatingCount = game.RatingCount ?? 0,
            TotalRating = game.TotalRating ?? 0,
            TotalRatingCount = game.TotalRatingCount ?? 0,
            UpdatedAt = game.UpdatedAt,
            RawData = game
        };
    }

    public static IEnumerable<GameEntity> ToMultipleEntities(this IEnumerable<IGDBGame> games)
    {
        var entities = games.Select(g => g.ToEntity());
        return entities;
    }
}