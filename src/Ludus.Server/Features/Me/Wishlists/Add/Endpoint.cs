using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Users.Models;
using Ludus.Server.Features.DataAccess;
using Marten;

namespace Me.Wishlists.Add;

public class Endpoint : Endpoint<AddWishlistItem>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/add/{GameId}");
        Group<MeWishlistGroup>();
    }

    public override async Task HandleAsync(AddWishlistItem req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var existing = await DBContext.Wishlists.FirstOrDefaultAsync(w =>
            w.UserId == userId && w.GameId == req.GameId
        );

        if (existing == null)
        {
            var newWishlistItem = new GameWishlist() { UserId = userId, GameId = req.GameId };
            DBContext.Wishlists.Add(newWishlistItem);
            await DBContext.SaveChangesAsync();
        }

        await SendOkAsync();
    }
}
