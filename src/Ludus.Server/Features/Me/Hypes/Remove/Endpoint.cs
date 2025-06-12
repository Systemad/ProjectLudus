using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.DataAccess;
using Microsoft.EntityFrameworkCore;

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
            .ExecuteDeleteAsync();

        await (rowsAffected == 0 ? SendNotFoundAsync() : SendOkAsync());
    }
}
