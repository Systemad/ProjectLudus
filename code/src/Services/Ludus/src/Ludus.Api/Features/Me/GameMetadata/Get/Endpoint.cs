using FastEndpoints;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Ludus.Api.Features.Auth.Extensions;
using Ludus.Api.Features.Common.Games.Mappers;
using Ludus.Api.Features.Common.Games.Models;
using Ludus.Api.Features.DataAccess;

namespace Me.GameMetadata.Get;

public class Endpoint : Endpoint<GeGameMetadataRequest, GameMetadataDto>
{
    public LudusContext _context { get; set; }
    public override void Configure()
    {
        Get("/{GameId}");
        Group<GameMetadataGroup>();
    }

    public override async Task HandleAsync(GeGameMetadataRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var hyped = await HypesHelper.IsHypedAsync(_context, userId, req.GameId, ct);
        var wishlisted = await WishlistsHelper.IsWishlistedAsync(_context, userId, req.GameId, ct);

        var metadata = GameMetadataMapper.MapToGameMetadata(req.GameId, wishlisted, hyped);

        await Send.OkAsync(
            metadata,
            cancellation: ct
        );
    }
}