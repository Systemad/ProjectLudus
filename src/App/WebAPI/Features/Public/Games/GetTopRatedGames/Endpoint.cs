using FastEndpoints;
using Marten;
using Marten.Pagination;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Public.Games.GetTopRatedGames;

public class Endpoint : Endpoint<GetTopRatedGamesRequest, PaginatedResponse<GameDto>>
{
    public IDocumentStore Store { get; set; }
    public IGameService GameService { get; set; }
    public LudusContext _context { get; set; }

    public override void Configure()
    {
        Get("/top");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTopRatedGamesRequest req, CancellationToken ct)
    {
        await using var session = Store.LightweightSession();

        var games = await session
            .Query<InsertIgdbGame>()
            .Where(x => x.GameType.Id == 0 && x.TotalRatingCount >= 90)
            .OrderByDescending(x => x.TotalRating)
            .ThenByDescending(x => x.TotalRatingCount)
            .ToPagedListAsync(req.PageNumber, req.PageSize, token: ct);

        HashSet<long> hypedGames = [];
        HashSet<long> wishlistedGames = [];

        if (User.Identity.IsAuthenticated)
        {
            var userId = User.GetUserId();

            wishlistedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(_context, userId, ct);
            hypedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId, ct);
        }

        var previews = await GameService.HydrateGamesAsync(games, wishlistedGames, hypedGames);
        await Send.OkAsync(
            new PaginatedResponse<GameDto>(
                previews,
                games.TotalItemCount,
                games.PageCount,
                games.PageSize,
                games.PageNumber,
                games.IsLastPage
            ),
            cancellation: ct
        );
    }
}