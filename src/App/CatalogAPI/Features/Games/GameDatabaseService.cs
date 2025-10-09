using CatalogAPI.Data;
using CatalogAPI.Data.Entities;
using CatalogAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;
using Shared.Features;

namespace CatalogAPI.Features.Games;

public class GameDatabaseService(SyncDbContext context)
{
    public async Task UpdateGameAsync(GameEntity game)
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

    public async Task DeleteGameAsync(long id)
    {
        var rowsAffected = await context.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
    }

    public Task InsertManyAsync<T>(List<T> source, CancellationToken ct)
        where T : class
    {
        if (source.Count == 0)
        {
            return Task.CompletedTask;
        }

        return context.BulkInsertAsync(source, ct);
    }
}
