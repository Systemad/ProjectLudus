using FastEndpoints;
using Ludus.Api.Features.Common.Endpoints;

namespace Me.Wishlists.GetAll;

public class GetWishlistedGamesRequest : IPaginationParameters
{
    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;
}
