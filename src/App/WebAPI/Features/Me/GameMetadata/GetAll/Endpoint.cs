using FastEndpoints;
using Marten;
using Marten.Pagination;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Me.GameMetadata.GetAll;

public class Endpoint : Endpoint<GeGameMetadataRequest, PaginatedResponse<GameMetadataDto>>
{
    public LudusContext _context { get; set; }
    public IDocumentStore Store { get; set; }

    public override void Configure()
    {
        Get("/all");
        Group<GameMetadataGroup>();
    }

    public override async Task HandleAsync(GeGameMetadataRequest req, CancellationToken ct)
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

        hypedGames.UnionWith(wishlistedGames);
        
        var games = await session
            .Query<IGDBGame>()
            .Where(g => g.Id.IsOneOf(hypedGames.ToArray()))
            .ToPagedListAsync(req.PageNumber, req.PageSize, token: ct);

        var previews = GameMetadataMapper.MapGamesToMetadatas(games.Select(x => x.Id), wishlistedGames, hypedGames);

        await Send.OkAsync(
            new PaginatedResponse<GameMetadataDto>(
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
