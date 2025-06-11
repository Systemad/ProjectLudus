using Ludus.Server.Features.Common.Lists;

namespace Me.Lists.Get;

public class GetListRequest
{
    public Guid ListId { get; set; }
}

public class GetListResponse
{
    public UserGameListDto List { get; set; }
}
