using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.DataAccess;

namespace Me.Hypes.Remove;

public class Endpoint : Endpoint<RemoveHypedItem>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/remove/{GameId}");
        Group<MeHypesGroup>();
    }

    public override async Task HandleAsync(RemoveHypedItem req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var rowsAffected = await DBContext
            .Hypes.Where(w => w.UserId == userId && w.GameId == req.GameId)
            .ExecuteDeleteAsync(cancellationToken: ct);

        await (rowsAffected == 0 ? SendNotFoundAsync(ct) : SendOkAsync(ct));
    }
}
