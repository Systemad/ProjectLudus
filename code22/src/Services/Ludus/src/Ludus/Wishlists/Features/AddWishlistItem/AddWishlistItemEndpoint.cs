using BuildingBlocks.Web;
using FastEndpoints;
using Ludus.Data;
using Ludus.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Wishlists.Features.AddWishlistItem;

public class AddWishlistItemRequest
{
    public long GameId { get; set; }
}


public class AddWishlistItemEndpoint : Endpoint<AddWishlistItemRequest>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/add/{GameId}");
        Description(x => x.Accepts<AddWishlistItemRequest>(), clearDefaults: true);
        Group<MeWishlistGroup>();
    }

    public override async Task HandleAsync(AddWishlistItemRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var existing = await DBContext.Wishlists.FirstOrDefaultAsync(
            w => w.UserId == userId && w.GameId == req.GameId,
            cancellationToken: ct
        );

        if (existing == null)
        {
            var newWishlistItem = new GameWishlist() { UserId = userId, GameId = req.GameId };
            DBContext.Wishlists.Add(newWishlistItem);
            await DBContext.SaveChangesAsync(ct);
        }

        await Send.OkAsync(cancellation: ct);
    }
}
