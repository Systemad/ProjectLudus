using CatalogAPI.Data.Features.Games;
using FastEndpoints;
using WebAPI.Features.Common.Endpoints;

namespace Features.Games.GetTopRatedGames;

public record GetTopRatedGamesResponse(PaginatedResponse<GameDto> Data);

public class GetTopRatedGamesRequest : IPaginationParameters
{
    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;
}
