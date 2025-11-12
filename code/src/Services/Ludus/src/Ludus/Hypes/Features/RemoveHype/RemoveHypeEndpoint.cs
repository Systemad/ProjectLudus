using BuildingBlocks.Web;
using FastEndpoints;
using Ludus.Data;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Hypes.Features.RemoveHype;

public class RemoveHypedItem
{
    public long GameId { get; set; }
}

public class RemoveHypeEndpoint : Endpoint<RemoveHypedItem>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/remove/{GameId}");
        Description(x => x.Accepts<RemoveHypedItem>(), clearDefaults: true);
        Group<MeHypesGroup>();
    }

    public override async Task HandleAsync(RemoveHypedItem req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var rowsAffected = await DBContext
            .Hypes.Where(w => w.UserId == userId && w.GameId == req.GameId)
            .ExecuteDeleteAsync(cancellationToken: ct);

        await (rowsAffected == 0 ? Send.NotFoundAsync(ct) : Send.OkAsync(cancellation: ct));
    }
}
