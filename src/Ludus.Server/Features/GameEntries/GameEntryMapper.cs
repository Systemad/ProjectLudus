using Ludus.Server.Features.Games;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.GameEntries;

public static class GameEntryMapper
{
    public static GameEntryPreviewDto ToGameEntryPreviewDto(
        this GameEntry gameEntry,
        GameDTO game
    ) =>
        new(
            gameEntry.Id,
            game,
            gameEntry.Status,
            gameEntry.StartDate,
            gameEntry.EndDate,
            gameEntry.UpdatedAt,
            gameEntry.Rating,
            gameEntry.Notes
        );

    public static GameEntryDto ToGameEntryDto(this GameEntry gameEntry) =>
        new(
            gameEntry.Id,
            gameEntry.GameId,
            gameEntry.Status,
            gameEntry.StartDate,
            gameEntry.EndDate,
            gameEntry.UpdatedAt,
            gameEntry.Rating,
            gameEntry.Notes
        );
}
