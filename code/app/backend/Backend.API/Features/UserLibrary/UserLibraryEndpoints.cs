using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Catalog.Queries;
using Microsoft.AspNetCore.Mvc;
using Play.Queries;
using Backend.API.Features;

namespace Backend.API.Features.UserLibrary;

public static class UserLibraryEndpoints
{
    public static void MapUserLibraryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/me/library")
            .WithTags("User Library")
            .RequireAuthorization();

        group
            .MapGet("/lists/{listId:guid}", GetListAsync)
            .Produces<UserLibraryListResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status503ServiceUnavailable)
            .ProducesValidationProblem();
        group
            .MapGet("/history", GetHistoryAsync)
            .Produces<UserLibraryHistoryResponse>()
            .Produces(StatusCodes.Status503ServiceUnavailable)
            .ProducesValidationProblem();
        group
            .MapGet("/games/{gameId}", GetGameAsync)
            .Produces<UserLibraryGameResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
        group
            .MapPost("/games/membership", GetMembershipAsync)
            .Produces<UserLibraryMembershipResponse>()
            .ProducesValidationProblem();
    }

    private static async Task<
        Results<ValidationProblem, NotFound, StatusCodeHttpResult, Ok<UserLibraryListResponse>>
    > GetListAsync(
        Guid listId,
        [AsParameters] UserLibraryCursorRequest request,
        ClaimsPrincipal principal,
        PlayListQueries listQueries,
        CatalogGameQueries catalogGames,
        CancellationToken ct
    )
    {
        var validationProblem = TryGetCursor(
            request.Cursor,
            UserLibraryCursorScopes.ListGames,
            out var cursor
        );
        if (validationProblem is not null)
            return validationProblem;

        SavedGameCursor? savedGameCursor = null;
        if (cursor is not null)
            savedGameCursor = new SavedGameCursor(cursor.Timestamp, cursor.Id);

        var page = await listQueries.GetAsync(
            UserId(principal),
            listId,
            savedGameCursor,
            request.PageSize,
            ct
        );

        if (page is null)
            return TypedResults.NotFound();

        var games = await catalogGames.GetGameCardsAsync(
            page.Games.Select(item => item.GameId).ToList(),
            ct
        );

        if (games.Count != page.Games.Count)
            return TypedResults.StatusCode(StatusCodes.Status503ServiceUnavailable);

        var nextCursor = page.NextCursor is null
            ? null
            : UserLibraryCursors.Encode(
                UserLibraryCursorScopes.ListGames,
                page.NextCursor.AddedAt,
                page.NextCursor.GameId
            );

        return TypedResults.Ok(
            new UserLibraryListResponse(
                new UserLibraryListSummary(
                    page.List.Id,
                    page.List.Name,
                    page.List.Visibility.ToString(),
                    page.List.IsDefault
                ),
                page.Games.Select(item => new UserLibrarySavedGameResponse(
                        item.AddedAt,
                        games[item.GameId]
                    ))
                    .ToList(),
                nextCursor
            )
        );
    }

    private static async Task<
        Results<ValidationProblem, StatusCodeHttpResult, Ok<UserLibraryHistoryResponse>>
    > GetHistoryAsync(
        [AsParameters] UserLibraryCursorRequest request,
        ClaimsPrincipal principal,
        PlayHistoryQueries historyQueries,
        CatalogGameQueries catalogGames,
        CancellationToken ct
    )
    {
        var validationProblem = TryGetCursor(
            request.Cursor,
            UserLibraryCursorScopes.History,
            out var cursor
        );
        if (validationProblem is not null)
            return validationProblem;

        HistoryCursor? historyCursor = null;
        if (cursor is not null)
            historyCursor = new HistoryCursor(cursor.Timestamp, cursor.Id);

        var history = await historyQueries.GetAsync(
            UserId(principal),
            historyCursor,
            request.PageSize,
            ct
        );
        var games = await catalogGames.GetGameCardsAsync(
            history.Items.Select(item => item.GameId).Distinct().ToList(),
            ct
        );

        if (games.Count != history.Items.Select(item => item.GameId).Distinct().Count())
            return TypedResults.StatusCode(StatusCodes.Status503ServiceUnavailable);

        var nextCursor = history.NextCursor is null
            ? null
            : UserLibraryCursors.Encode(
                UserLibraryCursorScopes.History,
                history.NextCursor.CreatedAt,
                history.NextCursor.Id
            );

        return TypedResults.Ok(
            new UserLibraryHistoryResponse(
                history
                    .Items.Select(item => new UserLibraryHistoryItemResponse(
                        item.Id,
                        item.ListId,
                        item.Action.ToString(),
                        item.CreatedAt,
                        games[item.GameId]
                    ))
                    .ToList(),
                nextCursor
            )
        );
    }

    private static async Task<Results<BadRequest, NotFound, Ok<UserLibraryGameResponse>>> GetGameAsync(
        string gameId,
        ClaimsPrincipal principal,
        CatalogGameQueries catalogGames,
        PlayMembershipQueries membershipQueries,
        CancellationToken ct
    )
    {
        if (!ApiId.TryParse(gameId, out var parsedGameId))
            return TypedResults.BadRequest();

        var cardsTask = catalogGames.GetGameCardsAsync([parsedGameId], ct);
        var membershipTask = membershipQueries.GetAsync(UserId(principal), [parsedGameId], ct);

        await Task.WhenAll(cardsTask, membershipTask);

        var cards = await cardsTask;
        var memberships = await membershipTask;

        return !cards.TryGetValue(parsedGameId, out var game)
            ? TypedResults.NotFound()
            : TypedResults.Ok(new UserLibraryGameResponse(game, memberships[parsedGameId]));
    }

    private static async Task<Results<ValidationProblem, Ok<UserLibraryMembershipResponse>>> GetMembershipAsync(
        GameIdsRequest request,
        ClaimsPrincipal principal,
        PlayMembershipQueries membershipQueries,
        CancellationToken ct
    )
    {
        if (request.GameIds.Count is < 1 or > 50)
        {
            return TypedResults.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    ["GameIds"] = ["Provide between 1 and 50 game IDs."],
                }
            );
        }

        if (request.GameIds.Any(gameId => !ApiId.TryParse(gameId, out _)))
        {
            return TypedResults.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    ["GameIds"] = ["Game IDs must be non-negative integers."],
                }
            );
        }

        var gameIds = request.GameIds.Select(ApiId.Parse).ToList();
        var memberships = await membershipQueries.GetAsync(UserId(principal), gameIds, ct);

        return TypedResults.Ok(
            new UserLibraryMembershipResponse(
                memberships
                    .Select(item => new UserLibraryMembershipItemResponse(
                        ApiId.Format(item.Key),
                        item.Value.IsWishlisted,
                        item.Value.ListIds
                    ))
                    .ToList()
            )
        );
    }

    private static Guid UserId(ClaimsPrincipal principal) =>
        Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private static ValidationProblem? TryGetCursor(
        string? value,
        string scope,
        out UserLibraryCursor? cursor
    )
    {
        if (value is null)
        {
            cursor = null;
            return null;
        }

        if (UserLibraryCursors.TryDecode(value, scope, out cursor))
            return null;

        return TypedResults.ValidationProblem(
            new Dictionary<string, string[]> { ["Cursor"] = ["Cursor is invalid."] }
        );
    }
}

public sealed class UserLibraryCursorRequest
{
    public string? Cursor { get; init; }

    [Range(1, 50)]
    public int PageSize { get; init; } = 20;
}

public sealed record GameIdsRequest
{
    [Required, MinLength(1), MaxLength(50)]
    public required IReadOnlyList<string> GameIds { get; init; }
}

public sealed record UserLibraryListSummary(
    Guid Id,
    string Name,
    string Visibility,
    bool IsDefault
);

public sealed record UserLibrarySavedGameResponse(DateTimeOffset AddedAt, GameCard Game);

public sealed record UserLibraryListResponse(
    UserLibraryListSummary List,
    IReadOnlyList<UserLibrarySavedGameResponse> Games,
    string? NextCursor
);

public sealed record UserLibraryHistoryItemResponse(
    long Id,
    Guid ListId,
    string Action,
    DateTimeOffset CreatedAt,
    GameCard Game
);

public sealed record UserLibraryHistoryResponse(
    IReadOnlyList<UserLibraryHistoryItemResponse> Items,
    string? NextCursor
);

public sealed record UserLibraryGameResponse(GameCard Game, GameMembership Membership);

public sealed record UserLibraryMembershipItemResponse(
    string GameId,
    bool IsWishlisted,
    IReadOnlyList<Guid> ListIds
);

public sealed record UserLibraryMembershipResponse(
    IReadOnlyList<UserLibraryMembershipItemResponse> Games
);
