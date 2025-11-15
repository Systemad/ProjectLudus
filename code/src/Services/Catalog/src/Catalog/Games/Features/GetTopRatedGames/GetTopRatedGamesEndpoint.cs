using BuildingBlocks.Core.Pagination;
using Catalog.Data;
using Catalog.Games.Dtos;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Games.Features.GetTopRatedGames;

public record GetTopRatedGamesResponse(PaginatedResponse<GameDto> Data);

public class GetTopRatedGamesRequest : IPaginationQuery
{
    [QueryParam]
    public int PageSize { get; set; } = 40;

    [QueryParam]
    public int PageNumber { get; set; } = 1;
}


public class GetTopRatedGamesEndpoint : Endpoint<GetTopRatedGamesRequest, PaginatedResponse<GameDto>>
{
    public CatalogDbContext DbContext { get; set; }

    public override void Configure()
    {
        Get("/top");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTopRatedGamesRequest req, CancellationToken ct)
    {
        var games = await DbContext
            .Games
            // TODO: USE RAW PARADEDB SYTNAX AND BOOST!!
            .Where(x => x.GameType == 0)
            .Take(20)
            .ToListAsync(cancellationToken: ct);

        await Send.OkAsync(
            new PaginatedResponse<GameDto>(new List<GameDto>(), 1, 1, 1, 2, false),
            cancellation: ct
        );
    }
}
