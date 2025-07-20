using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.DataAccess;

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

        var rowsAffected = await DBContext
            .Wishlists.Where(w => w.UserId == userId && w.GameId == req.GameId)
            .ExecuteDeleteAsync(cancellationToken: ct);

        await (rowsAffected == 0 ? SendNotFoundAsync(ct) : SendOkAsync(ct));
    }
}
