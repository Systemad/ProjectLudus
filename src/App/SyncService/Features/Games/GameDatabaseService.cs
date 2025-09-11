using Microsoft.EntityFrameworkCore;
using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;
using Shared.Features;
using SyncService.Data;
using SyncService.Utilities;

namespace SyncService.Features.Games;

public class GameDatabaseService
{
    private SyncDbContext _context;

    public GameDatabaseService(SyncDbContext context)
    {
        _context = context;
    }

    public async Task InsertGameBatchAsync(List<IGDBGame> games)
    {
        var entities = games.ToMultipleEntities();
        var dedupedGames = entities
            .GroupBy(g => g.Id)
            .Select(g => g.OrderByDescending(x => x.UpdatedAt).First())
            .ToList();

        await _context.Games.ExecuteBulkInsertAsync(dedupedGames, options =>
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

        await BulkInsertInBatchesAsync(inserData.GameModes, _context.GameModes);
        await BulkInsertInBatchesAsync(inserData.Genres, _context.Genres);
        await BulkInsertInBatchesAsync(inserData.Platforms, _context.Platforms);
        await BulkInsertInBatchesAsync(inserData.PlayerPerspectives, _context.PlayerPerspectives);
        await BulkInsertInBatchesAsync(inserData.GameEngines, _context.GameEngines);
        await BulkInsertInBatchesAsync(inserData.Themes, _context.Themes);
        await BulkInsertInBatchesAsync(inserData.Franchises, _context.Franchises);
        await BulkInsertInBatchesAsync(inserData.Keywords, _context.Keywords);
    }

    public async Task AddOrUpdateRangeAsync(
        IEnumerable<GameEntity> games,
        CancellationToken cancellationToken)
    {
        foreach (var game in games)
        {
            var existingGame = await _context.Games.FindAsync([game.Id], cancellationToken);
            if (existingGame == null)
            {
                _context.Games.Add(game);
            }
            else
            {
                _context.Entry(existingGame).CurrentValues.SetValues(game);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
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