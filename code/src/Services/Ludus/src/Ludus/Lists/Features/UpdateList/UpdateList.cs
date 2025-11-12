using BuildingBlocks.Web;
using FastEndpoints;
using Ludus.Data;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Lists.Features.UpdateList;

public class UpdateList : Endpoint<UpdateListRequest>
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
            .ExecuteUpdateAsync(
                updates =>
                    updates
                        .SetProperty(t => t.Name, req.Name)
                        .SetProperty(t => t.Public, req.Public),
                cancellationToken: ct
            );

        await (rowsAffected == 0 ?  Send.NotFoundAsync(ct) : Send.OkAsync(cancellation: ct));
    }
}