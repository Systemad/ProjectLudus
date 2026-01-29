using BuildingBlocks.Web;
using FastEndpoints;
using Ludus.Data;
using Ludus.Users.Models;

namespace Ludus.Lists.Features.CreateList;

public class CreateListEndpoint : Endpoint<CreateListRequest>
{
    public LudusContext DbContext { get; set; }

    public override void Configure()
    {
        Post("/create");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(CreateListRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var list = new GameList
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = req.Name,
            Public = req.Public,
        };
        DbContext.Lists.Add(list);
        await DbContext.SaveChangesAsync(ct);
        await Send.CreatedAtAsync($"/{list.Id}", cancellation: ct);
    }
}
