using Microsoft.EntityFrameworkCore;
using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;
using Shared.Features;
using SyncService.Data;
using SyncService.Data.Entities;
using SyncService.Utilities;

namespace SyncService.Features.Games;

public class GameDatabaseService(SyncDbContext context)
{
    public async Task InsertGameBatchAsync(List<IgdbGame> games)
    {
        var entities = games.ToMultipleEntities();
        var dedupedGames = entities
            .GroupBy(g => g.Id)
            .Select(g => g.OrderByDescending(x => x.UpdatedAt).First())
            .ToList();

        await context.Games.ExecuteBulkInsertAsync(dedupedGames, options =>
        {
            options.BatchSize = 10_000;
            options.OnProgress = rowsCopied => { Console.WriteLine($"Copied {rowsCopied} rows so far..."); };
        }, onConflict: new OnConflictOptions<GameEntity>
        {
            Match = e => new
            {
                e.Id,
            },

            Update = (inserted, excluded) => inserted,
            Where = (inserted, excluded) => excluded.Metadata.UpdatedAt > inserted.Metadata.UpdatedAt,
        });
    }


    
    public async Task UpdateGameAsync(
        GameEntity game)
    {
        var existingGame = await context.Games.FindAsync(game.Id);
        if (existingGame == null)
        {
            context.Games.Add(game);
        }
        else
        {
            context.Entry(existingGame).CurrentValues.SetValues(game);
        }

        await context.SaveChangesAsync();
    }


    public async Task DeleteGameAsync(
        long id)
    {
        var rowsAffected = await context
            .Games.Where(g => g.Id == id)
            .ExecuteDeleteAsync();
    }

    private async Task BulkInsertInBatchesAsync<T>(
        IEnumerable<T> items,
        DbSet<T> dbSet,
        int batchSize = 10_000
    ) where T : class
    {
        foreach (var batch in items.Chunk(batchSize))
        {
            await dbSet.ExecuteBulkInsertAsync(batch);
        }
    }
}