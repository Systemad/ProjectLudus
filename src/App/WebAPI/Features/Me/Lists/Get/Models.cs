using FastEndpoints;
using WebAPI.Features.Common.Endpoints;

namespace Me.Lists.Get;

public class GetListRequest : IPaginationParameters
{
    public Guid ListId { get; set; }

    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;
}
