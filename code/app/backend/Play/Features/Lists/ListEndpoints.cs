using System.Security.Claims;
using Play.Features.Auth;
using Play.Features.Lists.Common.Dtos;
using Play.Features.Lists.Common.Pagination;
using Play.Features.Lists.Create;
using Play.Features.Lists.Update;
using Play.Infrastructure.Persistence;
using Play.Queries;

namespace Play.Features.Lists;

public static class ListEndpoints
{
    public static void MapListEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api").RequireAuthorization().WithTags("Lists");

        group
            .MapGet(
                "/me/lists",
                async (ClaimsPrincipal principal, ListService service, CancellationToken ct) =>
                    await service.GetSummariesAsync(UserId(principal), ct)
            )
            .Produces<IEnumerable<ListSummaryResponse>>();

        group
            .MapPost(
                "/me/games/membership",
                async (
                    MembershipRequest request,
                    ClaimsPrincipal principal,
                    PlayMembershipQueries membershipQueries,
                    CancellationToken ct
                ) =>
                {
                    if (request.GameIds.Count is < 1 or > 50)
                    {
                        return Results.ValidationProblem(
                            new Dictionary<string, string[]>
                            {
                                ["GameIds"] = ["Provide between 1 and 50 game IDs."],
                            }
                        );
                    }

                    if (request.GameIds.Any(gameId => !ApiId.TryParse(gameId, out _)))
                    {
                        return Results.ValidationProblem(
                            new Dictionary<string, string[]>
                            {
                                ["GameIds"] = ["Game IDs must be non-negative integers."],
                            }
                        );
                    }

                    var gameIds = request.GameIds.Select(ApiId.Parse).ToList();

                    return Results.Ok(
                        await membershipQueries.GetAsync(UserId(principal), gameIds, ct)
                    );
                }
            )
            .Produces<IReadOnlyDictionary<string, GameMembership>>();

        group
            .MapPost(
                "/me/lists",
                async (
                    CreateListRequest request,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                    Results.Created(
                        "/api/me/lists",
                        AuthService.ToList(
                            await service.CreateAsync(UserId(principal), request, ct)
                        )
                    )
            )
            .Produces<ListSummaryResponse>(StatusCodes.Status201Created);

        group
            .MapGet(
                "/me/lists/{id:guid}",
                async (
                    Guid id,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    var list = await service.GetAsync(UserId(principal), id, true, ct);
                    return list is null ? Results.NotFound() : Results.Ok(AuthService.ToList(list));
                }
            )
            .Produces<ListSummaryResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet(
                "/me/lists/{id:guid}/games",
                async (
                    Guid id,
                    [AsParameters] PageRequest request,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    var games = await service.GetGamesAsync(
                        UserId(principal),
                        id,
                        true,
                        request.Page,
                        request.PageSize,
                        ct
                    );
                    return games is null ? Results.NotFound() : Results.Ok(games);
                }
            )
            .Produces<PagedListGamesResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet(
                "/me/games/{gameId}/lists",
                async (
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    return !ApiId.TryParse(gameId, out var parsedGameId)
                        ? Results.BadRequest()
                        : Results.Ok(
                            await service.GetMembershipAsync(UserId(principal), parsedGameId, ct)
                        );
                }
            )
            .Produces<GameListMembershipResponse>()
            .Produces(StatusCodes.Status400BadRequest);

        group
            .MapGet(
                "/me/lists/{id:guid}/history",
                async (
                    Guid id,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) => Results.Ok(await service.GetHistoryAsync(UserId(principal), id, ct))
            )
            .Produces<IEnumerable<ListHistoryEntryResponse>>();

        group
            .MapPut(
                "/me/lists/{id:guid}",
                async (
                    Guid id,
                    UpdateListRequest request,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                    await service.UpdateAsync(UserId(principal), id, request, ct)
                        ? Results.NoContent()
                        : Results.NotFound()
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapDelete(
                "/me/lists/{id:guid}",
                async (
                    Guid id,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                    await service.DeleteAsync(UserId(principal), id, ct)
                        ? Results.NoContent()
                        : Results.NotFound()
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapPut(
                "/me/lists/{id:guid}/games/{gameId}",
                async (
                    Guid id,
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                    !ApiId.TryParse(gameId, out var parsedGameId)
                        ? Results.BadRequest()
                        : await service.SetGameAsync(
                                UserId(principal),
                                id,
                                parsedGameId,
                                true,
                                ct
                            )
                            ? Results.NoContent()
                            : Results.NotFound()
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapDelete(
                "/me/lists/{id:guid}/games/{gameId}",
                async (
                    Guid id,
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                    !ApiId.TryParse(gameId, out var parsedGameId)
                        ? Results.BadRequest()
                        : await service.SetGameAsync(
                                UserId(principal),
                                id,
                                parsedGameId,
                                false,
                                ct
                            )
                            ? Results.NoContent()
                            : Results.NotFound()
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet(
                "/me/wishlist",
                async (ClaimsPrincipal principal, ListService service, CancellationToken ct) =>
                    AuthService.ToList(
                        (await service.GetMineAsync(UserId(principal), ct)).Single(x => x.IsDefault)
                    )
            )
            .Produces<ListSummaryResponse>();

        group
            .MapPut(
                "/me/wishlist/games/{gameId}",
                async (
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    if (!ApiId.TryParse(gameId, out var parsedGameId))
                        return Results.BadRequest();

                    var list = (await service.GetMineAsync(UserId(principal), ct)).Single(x =>
                        x.IsDefault
                    );
                    return await service.SetGameAsync(
                            UserId(principal),
                            list.Id,
                            parsedGameId,
                            true,
                            ct
                        )
                        ? Results.NoContent()
                        : Results.NotFound();
                }
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapDelete(
                "/me/wishlist/games/{gameId}",
                async (
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    if (!ApiId.TryParse(gameId, out var parsedGameId))
                        return Results.BadRequest();

                    var list = (await service.GetMineAsync(UserId(principal), ct)).Single(x =>
                        x.IsDefault
                    );
                    return await service.SetGameAsync(
                            UserId(principal),
                            list.Id,
                            parsedGameId,
                            false,
                            ct
                        )
                        ? Results.NoContent()
                        : Results.NotFound();
                }
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        endpoints
            .MapGet(
                "/api/lists/{id:guid}",
                async (Guid id, ListService service, CancellationToken ct) =>
                {
                    var list = await service.GetAsync(Guid.Empty, id, false, ct);
                    return list is null ? Results.NotFound() : Results.Ok(AuthService.ToList(list));
                }
            )
            .WithTags("Lists")
            .Produces<ListSummaryResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }

    private static Guid UserId(ClaimsPrincipal principal) =>
        Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
