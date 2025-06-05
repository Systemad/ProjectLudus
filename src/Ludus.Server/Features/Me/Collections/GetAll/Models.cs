using Ludus.Server.Features.Common.Endpoints;
using Ludus.Server.Features.Common.Games.Models;
using Ludus.Server.Features.Public.Games.Common;

namespace Ludus.Server.Features.Me.Collections.GetAll;

public record GetGameCollectionResponse(
    IEnumerable<GameDto> Items,
    long TotalItemCount,
    long PageCount,
    long PageNumer,
    bool IsLastPage
) : IPaginatedResponse<GameDto>;

public class GetGameCollectionRequest : IPaginationParameters
{
    //[FromQuery(Name = "ps")]
    public int PageSize { get; } = 40;

    //[FromQuery(Name = "pn")]
    public int PageNumber { get; } = 1;
}
