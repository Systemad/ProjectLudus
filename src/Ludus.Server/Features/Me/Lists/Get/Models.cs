using Ludus.Server.Features.Common.Lists;

namespace Ludus.Server.Features.Me.Lists.Get;

public class GetListRequest
{
    public Guid ListId { get; set; }
}

public class GetListResponse
{
    public UserGameListDto List { get; set; }
}
