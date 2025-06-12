using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Me.Lists.Update;

public class Endpoint : Endpoint<UpdateListRequest>
{
    public LudusContext DbContext { get; set; }

    public override void Configure()
    {
        Get("/update/{ListId}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(UpdateListRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var rowsAffected = await DbContext
            .Lists.Where(x => x.Id == req.ListId && x.UserId == userId)
            .ExecuteUpdateAsync(updates =>
                updates.SetProperty(t => t.Name, req.Name).SetProperty(t => t.Public, req.Public)
            );

        await (rowsAffected == 0 ? SendNotFoundAsync() : SendOkAsync());
    }
}
