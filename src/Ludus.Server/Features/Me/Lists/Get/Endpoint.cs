using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Lists.Services;
using Ludus.Server.Features.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Me.Lists.Get;

public class Endpoint : Endpoint<GetListRequest, GetListResponse>
{
    public LudusContext DbContext { get; set; }
    public IUserListService ListService { get; set; }

    public override void Configure()
    {
        Get("/{ListId}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(GetListRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var list = await DbContext.Lists.FirstOrDefaultAsync(x =>
            x.Id == req.ListId && x.UserId == userId
        );
        if (list is null)
        {
            ThrowError("List doesn't exist!");
        }

        var listPreview = await ListService.ToPreviewDtoAsync(list, User);
        await SendOkAsync(new GetListResponse { List = listPreview });
    }
}
