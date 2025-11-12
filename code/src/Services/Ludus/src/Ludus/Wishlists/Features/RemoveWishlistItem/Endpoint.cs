using BuildingBlocks.Web;
using FastEndpoints;
using Ludus.Data;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Wishlists.Features.RemoveWishlistItem;

public class RemoveWishlistItemRequest
{
    public long GameId { get; set; }
}


public class Endpoint : Endpoint<RemoveWishlistItemRequest>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/remove/{GameId}");
        Description(x => x.Accepts<RemoveWishlistItemRequest>(), clearDefaults: true);
        Group<MeWishlistGroup>();
    }

    public override async Task HandleAsync(RemoveWishlistItemRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var rowsAffected = await DBContext
            .Wishlists.Where(w => w.UserId == userId && w.GameId == req.GameId)
            .ExecuteDeleteAsync(cancellationToken: ct);

        await (rowsAffected == 0 ? Send.NotFoundAsync(ct) : Send.OkAsync(cancellation: ct));
    }
}
