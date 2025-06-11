using FastEndpoints;
using Ludus.Server.Features.Common.Lists;
using Ludus.Server.Features.Common.Lists.Services;
using Marten;

namespace Me.Lists.Get;

public class Endpoint : Endpoint<GetListRequest, GetListResponse>
{
    public IDocumentStore UserStore { get; set; }
    public IUserListService ListService { get; set; }

    public override void Configure()
    {
        Get("/{ListId}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(GetListRequest req, CancellationToken ct)
    {
        await using var session = UserStore.LightweightSession();
        var list = await session.LoadAsync<UserGameList>(req.ListId);
        if (list is null)
        {
            ThrowError("List doesn't exist!");
        }
        var listPreview = await ListService.ToPreviewDtoAsync(list, User);
        await SendOkAsync(new GetListResponse { List = listPreview });
    }
}
