using CatalogAPI.Common.Endpoints;
using CatalogAPI.Data.Features.Games;
using FastEndpoints;

namespace Features.Games.GetTopRatedGames;

public record GetTopRatedGamesResponse(PaginatedResponse<GameDto> Data);

public class GetTopRatedGamesRequest : IPaginationParameters
{
    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;
}
