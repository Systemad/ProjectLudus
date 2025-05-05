using Ludus.Server.Features.GameEntries.Handlers;
using Marten;

namespace Ludus.Server.Features.GameEntries.Services;

public class GameEntryService
{
    private IUserStore UserStore { get; set; }
    private IDocumentStore GameDb { get; set; }

    public GameEntryService(IUserStore db, IDocumentStore gameDb)
    {
        UserStore = db;
        GameDb = gameDb;
    }

    public async Task<GameEntry> UpsertGameEntryAsync(Guid userId, GameEntryQuery query)
    {
        await using var session = UserStore.LightweightSession();
        var gameEntry = await session
            .Query<GameEntry>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GameId == query.GameId);

        if (gameEntry is null)
        {
            gameEntry = new GameEntry
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
