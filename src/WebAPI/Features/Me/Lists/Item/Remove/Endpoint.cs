using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.DataAccess;

namespace Me.Lists.Item.Remove;

public class Endpoint : Endpoint<RemoveGameRequest>
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

        var listExists = await DbContext.Lists.AnyAsync(x =>
            x.Id == req.ListId && x.UserId == userId
        );
        if (!listExists)
        {
            ThrowError("List doesn't exist!");
        }

        var rowsAffected = await DbContext
            .ListItems.Where(gli => gli.GameListId == req.ListId && gli.GameId == req.GameId)
            .ExecuteDeleteAsync(ct);

        await (rowsAffected == 0 ? SendNotFoundAsync() : SendOkAsync());
    }
}
