using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Users.Models;
using Ludus.Server.Features.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Me.Hypes.Add;

public class Endpoint : Endpoint<AddHypedItem>
{
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Post("/add/{GameId}");
        Group<MeHypesGroup>();
    }

    public override async Task HandleAsync(AddHypedItem req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var existing = await DBContext.Hypes.FirstOrDefaultAsync(w =>
            w.UserId == userId && w.GameId == req.GameId
        );

        if (existing == null)
        {
            var newHypeItem = new GameHype() { UserId = userId, GameId = req.GameId };
            DBContext.Hypes.Add(newHypeItem);
            await DBContext.SaveChangesAsync();
        }

        await SendOkAsync();
    }
}
