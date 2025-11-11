using FastEndpoints;
using Ludus.Api.Features.Common.Endpoints;

namespace Me.Hypes.GetAll;

public class GetHypesGamesRequest : IPaginationParameters
{
    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;
}
