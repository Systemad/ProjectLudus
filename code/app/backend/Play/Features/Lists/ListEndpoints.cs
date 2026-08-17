using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
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
                async Task<Ok<List<ListSummaryResponse>>> (
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) => TypedResults.Ok(await service.GetSummariesAsync(UserId(principal), ct))
            )
            .Produces<IEnumerable<ListSummaryResponse>>();

        group
            .MapPost(
                "/me/games/membership",
                async Task<Results<ValidationProblem, Ok<IReadOnlyDictionary<long, GameMembership>>>> (
                    MembershipRequest request,
                    ClaimsPrincipal principal,
                    PlayMembershipQueries membershipQueries,
                    CancellationToken ct
                ) =>
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

                    return TypedResults.Ok(
                        await membershipQueries.GetAsync(UserId(principal), gameIds, ct)
                    );
                }
            )
            .Produces<IReadOnlyDictionary<string, GameMembership>>();

        group
            .MapPost(
                "/me/lists",
                async Task<Created<ListSummaryResponse>> (
                    CreateListRequest request,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    var list = await service.CreateAsync(UserId(principal), request, ct);
                    return TypedResults.Created($"/api/me/lists/{list.Id}", AuthService.ToList(list));
                }
            )
            .Produces<ListSummaryResponse>(StatusCodes.Status201Created);

        group
            .MapGet(
                "/me/lists/{id:guid}",
                async Task<Results<NotFound, Ok<ListSummaryResponse>>> (
                    Guid id,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    var list = await service.GetAsync(UserId(principal), id, true, ct);
                    return list is null
                        ? TypedResults.NotFound()
                        : TypedResults.Ok(AuthService.ToList(list));
                }
            )
            .Produces<ListSummaryResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet(
                "/me/lists/{id:guid}/games",
                async Task<Results<NotFound, Ok<PagedListGamesResponse>>> (
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
                    return games is null
                        ? TypedResults.NotFound()
                        : TypedResults.Ok(games);
                }
            )
            .Produces<PagedListGamesResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet(
                "/me/games/{gameId}/lists",
                async Task<Results<BadRequest, Ok<GameListMembershipResponse>>> (
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    return !ApiId.TryParse(gameId, out var parsedGameId)
                        ? TypedResults.BadRequest()
                        : TypedResults.Ok(
                            await service.GetMembershipAsync(UserId(principal), parsedGameId, ct)
                        );
                }
            )
            .Produces<GameListMembershipResponse>()
            .Produces(StatusCodes.Status400BadRequest);

        group
            .MapGet(
                "/me/lists/{id:guid}/history",
                async Task<Ok<List<ListHistoryEntryResponse>>> (
                    Guid id,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) => TypedResults.Ok(await service.GetHistoryAsync(UserId(principal), id, ct))
            )
            .Produces<IEnumerable<ListHistoryEntryResponse>>();

        group
            .MapPut(
                "/me/lists/{id:guid}",
                async Task<Results<NoContent, NotFound>> (
                    Guid id,
                    UpdateListRequest request,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                    await service.UpdateAsync(UserId(principal), id, request, ct)
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound()
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapDelete(
                "/me/lists/{id:guid}",
                async Task<Results<NoContent, NotFound>> (
                    Guid id,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                    await service.DeleteAsync(UserId(principal), id, ct)
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound()
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapPut(
                "/me/lists/{id:guid}/games/{gameId}",
                async Task<Results<BadRequest, NoContent, NotFound>> (
                    Guid id,
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                    !ApiId.TryParse(gameId, out var parsedGameId)
                        ? TypedResults.BadRequest()
                        : await service.SetGameAsync(
                                UserId(principal),
                                id,
                                parsedGameId,
                                true,
                                ct
                            )
                            ? TypedResults.NoContent()
                            : TypedResults.NotFound()
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapDelete(
                "/me/lists/{id:guid}/games/{gameId}",
                async Task<Results<BadRequest, NoContent, NotFound>> (
                    Guid id,
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                    !ApiId.TryParse(gameId, out var parsedGameId)
                        ? TypedResults.BadRequest()
                        : await service.SetGameAsync(
                                UserId(principal),
                                id,
                                parsedGameId,
                                false,
                                ct
                            )
                            ? TypedResults.NoContent()
                            : TypedResults.NotFound()
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet(
                "/me/wishlist",
                async Task<Ok<ListSummaryResponse>> (
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) => TypedResults.Ok(
                    AuthService.ToList(
                        (await service.GetMineAsync(UserId(principal), ct)).Single(x => x.IsDefault)
                    )
                )
            )
            .Produces<ListSummaryResponse>();

        group
            .MapPut(
                "/me/wishlist/games/{gameId}",
                async Task<Results<BadRequest, NoContent, NotFound>> (
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    if (!ApiId.TryParse(gameId, out var parsedGameId))
                        return TypedResults.BadRequest();

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
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound();
                }
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapDelete(
                "/me/wishlist/games/{gameId}",
                async Task<Results<BadRequest, NoContent, NotFound>> (
                    string gameId,
                    ClaimsPrincipal principal,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    if (!ApiId.TryParse(gameId, out var parsedGameId))
                        return TypedResults.BadRequest();

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
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound();
                }
            )
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        endpoints
            .MapGet(
                "/api/lists/{id:guid}",
                async Task<Results<NotFound, Ok<ListSummaryResponse>>> (
                    Guid id,
                    ListService service,
                    CancellationToken ct
                ) =>
                {
                    var list = await service.GetAsync(Guid.Empty, id, false, ct);
                    return list is null
                        ? TypedResults.NotFound()
                        : TypedResults.Ok(AuthService.ToList(list));
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
