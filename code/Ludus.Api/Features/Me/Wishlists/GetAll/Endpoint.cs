using FastEndpoints;
using Marten;
using Marten.Pagination;
using Me.Wishlists.Helpers;
using Shared.Features;
using Shared.Features.Games;
using Shared.Features.References.Platform;
using Ludus.Api.Features.Auth.Extensions;
using Ludus.Api.Features.Common.Endpoints;
using Ludus.Api.Features.DataAccess;

namespace Me.Wishlists.GetAll;

public class Endpoint : Endpoint<GetWishlistedGamesRequest, PaginatedResponse<WishlistItem>>
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

        var platformDict = new Dictionary<long, Platform>();
        
        var games = await session
            .Query<IGDBGameFlat>()
            .Include(platformDict).On(x => x.Platforms)
            .Where(g => wishlistedGames.Contains(g.Id))
            .ToPagedListAsync(req.PageNumber, req.PageSize, token: ct);

        var previews = games.Select(item =>
                item.ToDto(
                    platformDict,
                    wishlistedGames.Contains(item.Id)))
            .ToList();

        await Send.OkAsync(
            new PaginatedResponse<WishlistItem>(
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
