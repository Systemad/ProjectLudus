using FastEndpoints;
using Marten;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Me.GameMetadata.Get;

public class Endpoint : Endpoint<GeGameMetadataRequest, GameMetadataDto>
{
    public LudusContext _context { get; set; }
    public IDocumentStore Store { get; set; }

    public override void Configure()
    {
        Get("/{GameId}");
        Group<GameMetadataGroup>();
    }

    public override async Task HandleAsync(GeGameMetadataRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        await using var session = Store.QuerySession();
        var hyped = await HypesHelper.IsHypedAsync(_context, userId, req.GameId, ct);
        var wishlisted = await WishlistsHelper.IsWishlistedAsync(_context, userId, req.GameId, ct);

        var metadata = GameMetadataMapper.MapToGameMetadata(req.GameId, wishlisted, hyped);

        await Send.OkAsync(
            metadata,
            cancellation: ct
        );
    }
}