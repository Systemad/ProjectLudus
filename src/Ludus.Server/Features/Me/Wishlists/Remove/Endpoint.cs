using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Users.Models;
using Ludus.Server.Features.DataAccess;
using Marten;

namespace Me.Wishlists.Remove;

public class Endpoint : Endpoint<RemoveWishlistItem>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/remove/{GameId}");
        Group<MeWishlistGroup>();
    }

    public override async Task HandleAsync(RemoveWishlistItem req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var existing = await DBContext.Wishlists.FirstOrDefaultAsync(w =>
            w.UserId == userId && w.GameId == req.GameId
        );

        if (existing != null)
        {
            DBContext.Remove(existing);
            await DBContext.SaveChangesAsync();
        }

        await SendOkAsync();
    }
}
