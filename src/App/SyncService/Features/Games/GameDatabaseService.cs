using Microsoft.EntityFrameworkCore;
using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;
using Shared.Features;
using SyncService.Data;
using SyncService.Utilities;

namespace SyncService.Features.Games;

public class GameDatabaseService(SyncDbContext context)
{
    public async Task InsertGameBatchAsync(List<IGDBGame> games)
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
            Where = (inserted, excluded) => excluded.RawData.UpdatedAt > inserted.RawData.UpdatedAt,
        });
    }

    public async Task InsertFiltersAsync(
        List<IGDBGame> games
    )
    {
        var inserData = new InsertData();
        inserData.GameModes.AddRange(CommonUtilities.GetDistinctEntities(games, g => g.GameModes));
        inserData.Genres.AddRange(CommonUtilities.GetDistinctEntities(games, g => g.Genres));
        inserData.Platforms.AddRange(CommonUtilities.GetDistinctEntities(games, g => g.Platforms));
        inserData.PlayerPerspectives.AddRange(CommonUtilities.GetDistinctEntities(games, g => g.PlayerPerspectives));
        inserData.GameEngines.AddRange(CommonUtilities.GetDistinctEntities(games, g => g.GameEngines));
        inserData.Themes.AddRange(CommonUtilities.GetDistinctEntities(games, g => g.Themes));
        inserData.Franchises.AddRange(CommonUtilities.GetDistinctEntities(games, g => g.Franchises));
        inserData.Keywords.AddRange(CommonUtilities.GetDistinctEntities(games, g => g.Keywords));

        await BulkInsertInBatchesAsync(inserData.GameModes, context.GameModes);
        await BulkInsertInBatchesAsync(inserData.Genres, context.Genres);
        await BulkInsertInBatchesAsync(inserData.Platforms, context.Platforms);
        await BulkInsertInBatchesAsync(inserData.PlayerPerspectives, context.PlayerPerspectives);
        await BulkInsertInBatchesAsync(inserData.GameEngines, context.GameEngines);
        await BulkInsertInBatchesAsync(inserData.Themes, context.Themes);
        await BulkInsertInBatchesAsync(inserData.Franchises, context.Franchises);
        await BulkInsertInBatchesAsync(inserData.Keywords, context.Keywords);
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