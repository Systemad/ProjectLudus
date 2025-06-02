using Ludus.Server.Features.Collection.Handlers;
using Ludus.Server.Features.Shared;
using Marten;

namespace Ludus.Server.Features.Collection.Services;

public class GameCollectionService
{
    private IDocumentStore UserStore { get; set; }
    private IGameStore GameDb { get; set; }

    public GameCollectionService(IDocumentStore db, IGameStore gameDb)
    {
        UserStore = db;
        GameDb = gameDb;
    }

    public async Task<UserGameData> UpsertGameEntryAsync(
        Guid userId,
        UpsertGameCollectionQuery query
    )
    {
        await using var session = UserStore.LightweightSession();
        var gameEntry = await session
            .Query<UserGameData>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == query.GameId);

        if (gameEntry is null)
        {
            gameEntry = new UserGameData
            {
                UserId = userId,
                GameId = query.GameId,
                Status = query.Status,
                UpdatedAt = DateTime.UtcNow,
                Rating = query.Rating,
                StartDate = query.StartDate,
                EndDate = query.EndDate,
                Notes = query.Notes,
            };
        }
        else
        {
            gameEntry.UpdatedAt = DateTime.UtcNow;
            gameEntry.Status = query.Status;
            gameEntry.Rating = query.Rating;
            gameEntry.StartDate = query.StartDate;
            gameEntry.EndDate = query.EndDate;
            gameEntry.Notes = query.Notes;
        }

        if (query.Status == GameStatus.InProgress)
            gameEntry.StartDate = new DateTimeOffset().UtcDateTime;
        session.Store(gameEntry);
        await session.SaveChangesAsync();

        return gameEntry;
    }
}
