using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Ludus.Server.Features.Collection.Services;
using Ludus.Server.Features.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Collection.Handlers;

public record UpsertGameCollectionQuery(
    long GameId,
    GameStatus Status,
    DateTime? StartDate,
    DateTime? EndDate,
    [Range(0, 5)] int? Rating,
    string? Notes
);

public static class CreateOrUpdateGameStatusAsync
{
    public static async Task<Results<Ok<GameCollectionDto>, ProblemHttpResult>> Handler(
        [FromServices] GameCollectionService gameCollectionService,
        ClaimsPrincipal user,
        //long gameId,
        [FromBody] UpsertGameCollectionQuery query
    )
    {
        var userId = Guid.Parse(user.Identity.Name);

        if (query.GameId == 0)
        {
            return TypedResults.Problem(
                type: "Bad request",
                title: "Null GameId",
                detail: "Game ID cannot be null",
                statusCode: StatusCodes.Status400BadRequest
            );
            //return Problem()M
            //return TypedResults.BadRequest();
        }
        var entry = await gameCollectionService.UpsertGameEntryAsync(userId, query);

        return TypedResults.Ok(entry.ToGameEntryDto());
    }
}
