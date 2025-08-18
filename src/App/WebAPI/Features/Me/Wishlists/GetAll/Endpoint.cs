using FastEndpoints;
using Marten;
using Marten.Pagination;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Me.Wishlists.GetAll;

public class Endpoint : Endpoint<GetWishlistedGamesRequest, PaginatedResponse<GamePreviewDto>>
{
    public LudusContext _context { get; set; }
    public IDocumentStore Store { get; set; }
    
    public override void Configure()
    {
        Get("/all");
        Group<MeWishlistGroup>();
    }

    public override async Task HandleAsync(GetWishlistedGamesRequest req, CancellationToken ct)
    {
        await using var session = Store.QuerySession();
        var userId = User.GetUserId();

        HashSet<long> wishlistedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(
            _context,
            userId,
            ct
        );

        HashSet<long> hypedGames = await HypesHelper.GetHypedGamesByIdsAsync(
            _context,
            userId,
            wishlistedGames,
            ct
        );

        var platformDict = new Dictionary<long, Platform>();
        
        var games = await session
            .Query<IGDBGameFlat>()
            .Include(platformDict).On(x => x.Platforms)
            .Where(g => hypedGames.Contains(g.Id))
            .ToPagedListAsync(req.PageNumber, req.PageSize, token: ct);

        var previews = games.Select(item =>
                item.ToGamePreviewDto(
                    platformDict,
                    wishlistedGames.Contains(item.Id),
                    hypedGames.Contains(item.Id)))
            .ToList();

        await Send.OkAsync(
            new PaginatedResponse<GamePreviewDto>(
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
