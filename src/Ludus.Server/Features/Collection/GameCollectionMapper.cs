using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Collection;

public static class GameCollectionMapper
{
    public static UserGameDataDto ToGameCollectionDto(this UserGameData userGameData) =>
        new(
            userGameData.Id,
            userGameData.GameId,
            userGameData.Status,
            userGameData.StartDate,
            userGameData.EndDate,
            userGameData.UpdatedAt,
            userGameData.Rating,
            userGameData.Notes
        );

    public static UserGameDataDto ToGameEntryDto(this UserGameData userGameData) =>
        new(
            userGameData.Id,
            userGameData.GameId,
            userGameData.Status,
            userGameData.StartDate,
            userGameData.EndDate,
            userGameData.UpdatedAt,
            userGameData.Rating,
            userGameData.Notes
        );
}
