using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Lists;
using Ludus.Server.Features.Common.Lists.Services;
using Marten;

namespace Ludus.Server.Features.Me.Lists.GetAll;

public class Endpoint : EndpointWithoutRequest<GetMyListsResponse>
{
    public UserListService ListService { get; set; }
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Get("/{all}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetUserId();
        await using var session = UserStore.LightweightSession();
        var lists = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var listDtos = new List<UserGameListDto>();
        foreach (var item in lists)
        {
            var dto = await ListService.ToPreviewDtoAsync(item, User);
            listDtos.Add(dto);
        }

        await SendAsync(new GetMyListsResponse() { Lists = listDtos });
    }
}
