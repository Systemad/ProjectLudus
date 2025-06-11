using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.DataAccess;
using Marten;

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

        var existing = await DBContext.GameHypes.FirstOrDefaultAsync(w =>
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
