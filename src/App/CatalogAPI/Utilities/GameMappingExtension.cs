using CatalogAPI.Data.Entities;
using NodaTime;
using Shared.Features;

namespace CatalogAPI.Utilities;

// to mapping? or not? just one ProcessResult?
public static class GameMappingExtension
{
    public static GameEntity ToEntity(this IgdbGame game)
    {
        return new GameEntity()
        {
            Id = game.Id,
            Name = game.Name,
            FirstReleaseDate = Instant.FromUnixTimeSeconds(game.FirstReleaseDate),
            GameType = game.IgdbGameType.Id,
            //Platforms = game.Platforms?.Select(x => x.Id).ToArray() ?? [],
            //GameEngines = game.GameEngines?.Select(x => x.Id).ToArray() ?? [],
            //Genres = game.Genres?.Select(x => x.Id).ToArray() ?? [],
            //Themes = game.Themes?.Select(x => x.Id).ToArray() ?? [],
            Rating = game.Rating ?? 0,
            RatingCount = game.RatingCount ?? 0,
            TotalRating = game.TotalRating ?? 0,
            TotalRatingCount = game.TotalRatingCount ?? 0,
            UpdatedAt = Instant.FromUnixTimeSeconds(game.UpdatedAt),
            Metadata = game,
        };
    }

    public static IEnumerable<GameEntity> ToMultipleEntities(this IEnumerable<IgdbGame> games)
    {
        var entities = games.Select(g => g.ToEntity());
        return entities;
    }
}
