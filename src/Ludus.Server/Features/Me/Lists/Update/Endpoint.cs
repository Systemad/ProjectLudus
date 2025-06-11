using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Lists;
using Marten;

namespace Me.Lists.Update;

public class Endpoint : Endpoint<UpdateListRequest>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Get("/update/{ListId}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(UpdateListRequest req, CancellationToken ct)
    {
        if (req.Name.Length < 3)
        {
            ThrowError("Name must be longer than 3 characters!");
        }

        var userId = User.GetUserId();
        await using var session = UserStore.LightweightSession();
        var updateList = await session
            .Query<UserGameList>()
            .FirstOrDefaultAsync(x => x.Id == req.ListId && x.UserId == userId);
        if (updateList is null)
        {
            ThrowError("List not found!");
        }

        updateList.Name = req.Name;
        updateList.Public = req.Public;
        session.Store(updateList);
        await session.SaveChangesAsync();
        await SendOkAsync();
    }
}
