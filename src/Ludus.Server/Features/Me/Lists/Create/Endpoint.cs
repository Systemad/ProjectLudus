using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Lists;
using Marten;

namespace Ludus.Server.Features.Me.Lists.Create;

public class Endpoint : Endpoint<CreateListRequest>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Post("/create");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(CreateListRequest req, CancellationToken ct)
    {
        if (req.Name.Length < 3)
        {
            ThrowError("Name must be longer than 3 characters!");
        }

        var userId = User.GetUserId();
        await using var session = UserStore.LightweightSession();
        var list = new UserGameList
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = req.Name,
            Public = req.Public,
        };
        session.Store(list);
        await session.SaveChangesAsync();

        await SendCreatedAtAsync($"/{list.Id}");
    }
}
