using WebAPI.Features.Common.Lists;

namespace Me.Lists.Get;

public class GetListRequest
{
    public Guid ListId { get; set; }
}

public class GetListResponse
{
    public GameListDto List { get; set; }
}
