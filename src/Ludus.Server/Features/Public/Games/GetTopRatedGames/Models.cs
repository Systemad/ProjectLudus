using Ludus.Server.Features.Common.Endpoints;
using Ludus.Server.Features.Common.Games.Models;
using Ludus.Server.Features.Public.Games.Common;

namespace Ludus.Server.Features.Public.Games.GetTopRatedGames;

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
