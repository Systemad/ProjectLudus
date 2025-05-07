using Ludus.Server.Features.Collection.Handlers;
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

    public async Task<GameCollection> UpsertGameEntryAsync(Guid userId, GameEntryQuery query)
    {
        await using var session = UserStore.LightweightSession();
        var gameEntry = await session
            .Query<GameCollection>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == query.GameId);

        if (gameEntry is null)
        {
            gameEntry = new GameCollection
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
        session.Store(gameEntry);
        await session.SaveChangesAsync();

        return gameEntry;
    }
}
