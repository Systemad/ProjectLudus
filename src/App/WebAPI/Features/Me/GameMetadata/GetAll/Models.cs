using FastEndpoints;
using WebAPI.Features.Common.Endpoints;

namespace Me.GameMetadata.GetAll;

public class GeGameMetadataRequest : IPaginationParameters
{
    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;
}
