using Catalog.Games.Models;
using IGDB.Models;
using NodaTime;

namespace Catalog.Games.Features;

public static class GameMappingExtension
{
    public static GameEntity ToEntity(this Game game)
    {
        if (game.Id is not null)
        {
            return new GameEntity()
            {
                Id = game.Id.Value,
                Name = game.Name,
                FirstReleaseDate = Instant.FromDateTimeOffset(game.FirstReleaseDate.Value),
                //GameType = game.GameType.Id,
                //Platforms = game.Platforms?.Select(x => x.Id).ToArray() ?? [],
                //GameEngines = game.GameEngines?.Select(x => x.Id).ToArray() ?? [],
                //Genres = game.Genres?.Select(x => x.Id).ToArray() ?? [],
                //Themes = game.Themes?.Select(x => x.Id).ToArray() ?? [],
                Rating = game.Rating ?? 0,
                RatingCount = game.RatingCount ?? 0,
                TotalRating = game.TotalRating ?? 0,
                TotalRatingCount = game.TotalRatingCount ?? 0,
                UpdatedAt = Instant.FromDateTimeOffset(game.UpdatedAt.Value),
                Metadata = game,
            };
        }
        return null;
    }

    public static IEnumerable<GameEntity> ToMultipleEntities(this IEnumerable<Game> games)
    {
        var entities = games.Select(g => g.ToEntity());
        return entities;
    }
}
