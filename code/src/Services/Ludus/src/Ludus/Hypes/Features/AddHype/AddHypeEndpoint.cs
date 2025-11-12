using BuildingBlocks.Web;
using FastEndpoints;
using Ludus.Data;
using Ludus.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Hypes.Features.AddHype;

public class AddHypeRequest
{
    public long GameId { get; set; }
}

public class AddHypeEndpoint : Endpoint<AddHypeRequest>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/add/{GameId}");
        Description(x => x.Accepts<AddHypeRequest>(), clearDefaults: true);
        Group<MeHypesGroup>();
    }

    public override async Task HandleAsync(AddHypeRequest req, CancellationToken ct)
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