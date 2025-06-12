using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Users.Models;
using Ludus.Server.Features.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Me.Lists.Item.Add;

public class Endpoint : Endpoint<AddGameToListRequest>
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

        var list = await DbContext.Lists.AnyAsync(x => x.Id == req.ListId && x.UserId == userId);

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
        await DbContext.SaveChangesAsync();
        await SendOkAsync();
    }
}
