using BuildingBlocks.Core.Pagination;
using Catalog.Api.Data.Features.Games;
using FastEndpoints;

namespace Features.Games.GetTopRatedGames;

public record GetTopRatedGamesResponse(PaginatedResponse<GameDto> Data);

public class GetTopRatedGamesRequest : IPaginationQuery
{
    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;
}
