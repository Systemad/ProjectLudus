using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace API.Features.IGDB;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games")
            .WithTags("Games")
            .WithOpenApi();

        group.MapPost("/", GetGamesByIds);
        group.MapGet("/{id:long}", GetGamesById);
    }

    private static async Task<Ok<List<Game>>> GetGamesByIds(AppDbContext db, [FromBody] List<long> ids)
    {
        var results = await db.Games.Where(x => ids.Contains(x.Id)).ToListAsync();
        return TypedResults.Ok(results);
    }

    private static async Task<Results<Ok<Game>, NotFound>> GetGamesById(AppDbContext db, long id)
    {
        var game =  await db.Games.FindAsync(id);
        if(game is null)
            TypedResults.NotFound();

        return TypedResults.Ok(game);
    }
}