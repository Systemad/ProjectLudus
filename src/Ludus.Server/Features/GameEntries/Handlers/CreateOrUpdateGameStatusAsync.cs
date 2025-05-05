using System.Security.Claims;
using Ludus.Server.Features.GameEntries.Services;
using Ludus.Server.Features.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.GameEntries.Handlers;

public record GameEntryQuery(
    Guid Id,
    long GameId,
    GameStatus Status,
    DateTime? StartDate,
    DateTime? EndDate,
    int? Rating,
    string? Notes
);

public static class CreateOrUpdateGameStatusAsync
{
    public static async Task<Results<Ok<GameEntryDto>, BadRequest>> Handler(
        [FromServices] GameEntryService gameEntryService,
        ClaimsPrincipal user,
        long gameId,
        [FromBody] GameEntryQuery query
    )
    {
        var userId = Guid.Parse(user.Identity.Name);

        var entry = await gameEntryService.UpsertGameEntryAsync(userId, query);

        return TypedResults.Ok(entry.ToGameEntryDto());
    }
}
