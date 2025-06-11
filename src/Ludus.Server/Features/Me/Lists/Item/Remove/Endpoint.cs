using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Lists;
using Marten;

namespace Me.Lists.Item.Remove;

public class Endpoint : Endpoint<RemoveGameRequest>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Get("/remove/{ListId}/{GameId}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(RemoveGameRequest req, CancellationToken ct)
    {
        await using var session = UserStore.LightweightSession();
        var userId = User.GetUserId();

        var list = await session
            .Query<UserGameList>()
            .FirstOrDefaultAsync(x => x.Id == req.ListId && x.UserId == userId);

        if (list is null)
        {
            ThrowError("List doesn't exist!");
        }

        if (list.Games.Contains(req.GameId))
        {
            ThrowError("Game is already added to list!");
        }

        list.Games.Remove(req.GameId);
        session.Store(list);
        await session.SaveChangesAsync();

        await SendOkAsync();
    }
}
