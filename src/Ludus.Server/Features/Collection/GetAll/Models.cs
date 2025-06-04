using Ludus.Server.Features.Games;
using Ludus.Server.Features.Games.Common;
using Ludus.Server.Features.Shared;

namespace Ludus.Server.Features.Collection.GetAll;

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
