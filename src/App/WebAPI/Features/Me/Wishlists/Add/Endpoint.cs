using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Users.Models;
using WebAPI.Features.DataAccess;

namespace Me.Wishlists.Add;

public class Endpoint : Endpoint<AddWishlistItem>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/add/{GameId}");
        Description(x => x.Accepts<AddWishlistItem>(), clearDefaults: true);
        Group<MeWishlistGroup>();
    }

    public override async Task HandleAsync(AddWishlistItem req, CancellationToken ct)
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
