using System.Security.Claims;
using Ludus.Server.Features.Collection.Services;
using Ludus.Server.Features.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Collection.Handlers;

public record GameEntryQuery(
    long GameId,
    GameStatus Status,
    DateTime? StartDate,
    DateTime? EndDate,
    int? Rating,
    string? Notes
);

public static class CreateOrUpdateGameStatusAsync
{
    public static async Task<Results<Ok<GameCollectionDto>, BadRequest>> Handler(
        [FromServices] GameCollectionService gameCollectionService,
        ClaimsPrincipal user,
        //long gameId,
        [FromBody] GameEntryQuery query
    )
    {
        var userId = Guid.Parse(user.Identity.Name);

        var entry = await gameCollectionService.UpsertGameEntryAsync(userId, query);

        return TypedResults.Ok(entry.ToGameEntryDto());
    }
}
