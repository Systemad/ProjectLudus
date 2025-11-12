using BuildingBlocks.Web;
using FastEndpoints;
using Ludus.Data;
using Ludus.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Lists.Features.AddListItem;

public class AddGameToListRequest
{
    public Guid ListId { get; set; }
    public long GameId { get; set; }
}


public class AddListItemEndpoint : Endpoint<AddGameToListRequest>
{
    public LudusContext DbContext { get; set; }

    public override void Configure()
    {
        Get("/add/{listId}/{gameId}");
        Group<MeListsGroup>();
        // TODO: fix?
        //PreProcessor<CheckGamePreProcessor>();
    }

    public override async Task HandleAsync(AddGameToListRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var list = await DbContext.Lists.AnyAsync(
            x => x.Id == req.ListId && x.UserId == userId,
            cancellationToken: ct
        );

        if (!list)
        {
            ThrowError("List doesn't exist!");
        }

        var alreadyAdded = DbContext.ListItems.Any(x =>
            x.GameId == req.GameId && x.GameListId == req.ListId
        );

        if (alreadyAdded)
        {
            ThrowError("Game already exists in the list!");
        }

        DbContext.ListItems.Add(
            new GameListItem
            {
                Id = Guid.NewGuid(),
                GameId = req.GameId,
                AddedAt = DateTimeOffset.UtcNow,
                GameListId = req.ListId,
            }
        );
        await DbContext.SaveChangesAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}
