using FastEndpoints;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games.Models;

namespace Public.Games.GetTopRatedGames;

public record GetTopRatedGamesResponse(PaginatedResponse<GamePreviewDto> Data);

public class GetTopRatedGamesRequest : IPaginationParameters
{
    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;
}
