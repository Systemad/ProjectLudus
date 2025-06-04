using Ludus.Server.Features.Games.Common;
using Ludus.Server.Features.Shared;

namespace Ludus.Server.Features.Games.GetTopRatedGames;

public record GetTopRatedGamesResponse(
    IEnumerable<GameDto> Items,
    long TotalItemCount,
    long PageCount,
    long PageNumer,
    bool IsLastPage
) : IPaginatedResponse<GameDto>;

public class GetTopRatedGamesRequest : IPaginationParameters
{
    public int PageSize { get; set; } = 40;
    public int PageNumber { get; set; } = 1;
}
