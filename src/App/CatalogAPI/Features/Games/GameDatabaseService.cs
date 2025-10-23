using CatalogAPI.Data;
using CatalogAPI.Data.Features.Games;
using CatalogAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;
using Shared.Features;

namespace CatalogAPI.Features.Games;

public class GameDatabaseService(CatalogContext context)
{
    public async Task UpdateGameAsync(GameEntity gameEntity)
    {
        var existingGame = await context.Games.FindAsync(gameEntity.Id);
        if (existingGame == null)
        {
            context.Games.Add(gameEntity);
        }
        else
        {
            context.Entry(existingGame).CurrentValues.SetValues(gameEntity);
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
