using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Ludus.Api.Features.Auth.Extensions;
using Ludus.Api.Features.Common.Users.Models;
using Ludus.Api.Features.DataAccess;

namespace Me.Hypes.Add;

public class Endpoint : Endpoint<AddHypedItemRequest>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/add/{GameId}");
        Description(x => x.Accepts<AddHypedItemRequest>(), clearDefaults: true);
        Group<MeHypesGroup>();
    }

    public override async Task HandleAsync(AddHypedItemRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var existing = await DBContext.Hypes.FirstOrDefaultAsync(
            w => w.UserId == userId && w.GameId == req.GameId,
            cancellationToken: ct
        );

        if (existing == null)
        {
            var newHypeItem = new GameHype() { UserId = userId, GameId = req.GameId };
            DBContext.Hypes.Add(newHypeItem);
            await DBContext.SaveChangesAsync(ct);
        }

        await Send.OkAsync(cancellation: ct);
    }
}