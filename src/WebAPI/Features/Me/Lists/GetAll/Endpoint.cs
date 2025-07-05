using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Lists.Services;
using WebAPI.Features.DataAccess;

namespace Me.Lists.GetAll;

public class Endpoint : EndpointWithoutRequest<GetMyListsResponse>
{
    public IUserListService ListService { get; set; }
    public LudusContext DbContext { get; set; }

    public override void Configure()
    {
        Get("/{all}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var lists = await DbContext.Lists.Where(x => x.Id == userId).ToListAsync();
        var listTasks = lists.Select(x => ListService.ToPreviewDtoAsync(x, User));
        var previews = await Task.WhenAll(listTasks);
        await SendAsync(new GetMyListsResponse() { Lists = previews });
    }
}
