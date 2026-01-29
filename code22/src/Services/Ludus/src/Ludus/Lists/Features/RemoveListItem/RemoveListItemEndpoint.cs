using BuildingBlocks.Web;
using FastEndpoints;
using Ludus.Data;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Lists.Features.RemoveListItem;

public class RemoveGameRequest
{
    public Guid ListId { get; set; }
    public long GameId { get; set; }
}

public class RemoveListItemEndpoint : Endpoint<RemoveGameRequest>
{
    public LudusContext DbContext { get; set; }

    public override void Configure()
    {
        Get("/remove/{ListId}/{GameId}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(RemoveGameRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var listExists = await DbContext.Lists.AnyAsync(
            x => x.Id == req.ListId && x.UserId == userId,
            cancellationToken: ct
        );
        if (!listExists)
        {
            ThrowError("List doesn't exist!");
        }

        var rowsAffected = await DbContext
            .ListItems.Where(gli => gli.GameListId == req.ListId && gli.GameId == req.GameId)
            .ExecuteDeleteAsync(ct);

        await (rowsAffected == 0 ? Send.NotFoundAsync(ct) : Send.OkAsync(cancellation: ct));
    }
}
