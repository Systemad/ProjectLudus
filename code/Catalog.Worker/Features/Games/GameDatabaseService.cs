using CatalogAPI.Data.Features.Games;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Worker.Features.Games;

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


}
