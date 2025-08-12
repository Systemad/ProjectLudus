using FastEndpoints;
using Marten;
using Marten.Pagination;
using Me.Hypes;
using Me.Hypes.GetAll;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Me.Hyped.GetAll;

public class Endpoint : Endpoint<GetHypesGamesRequest, PaginatedResponse<GameDto>>
{
    public LudusContext _context { get; set; }
    public IDocumentStore Store { get; set; }
    public IGameService GameService { get; set; }

    public override void Configure()
    {
        Get("/all");
        Group<MeHypesGroup>();
    }

    public override async Task HandleAsync(GetHypesGamesRequest req, CancellationToken ct)
    {
        await using var session = Store.QuerySession();

        var userId = User.GetUserId();

        HashSet<long> hypedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId, ct);

        HashSet<long> wishlistedGames = await WishlistsHelper.GetWishlistedGamesByIdsAsync(
            _context,
            userId,
            hypedGames,
            ct
        );

        var games = await session
            .Query<InsertIGDBGame>()
            .Where(g => g.Id.IsOneOf(hypedGames.ToArray()))
            .ToPagedListAsync(req.PageNumber, req.PageSize, token: ct);

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
