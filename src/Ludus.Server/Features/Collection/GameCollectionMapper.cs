using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Collection;

public static class GameCollectionMapper
{
    public static GameCollectionPreviewDto ToGameEntryPreviewDto(
        this GameCollection gameCollection,
        GameDTO game
    ) =>
        new(
            gameCollection.Id,
            game,
            gameCollection.Status,
            gameCollection.StartDate,
            gameCollection.EndDate,
            gameCollection.UpdatedAt,
            gameCollection.Rating,
            gameCollection.Notes
        );

    public static GameCollectionDto ToGameEntryDto(this GameCollection gameCollection) =>
        new(
            gameCollection.Id,
            gameCollection.GameId,
            gameCollection.Status,
            gameCollection.StartDate,
            gameCollection.EndDate,
            gameCollection.UpdatedAt,
            gameCollection.Rating,
            gameCollection.Notes
        );
}
