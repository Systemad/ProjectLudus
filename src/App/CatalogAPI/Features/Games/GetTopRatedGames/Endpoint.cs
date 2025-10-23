using CatalogAPI.Data;
using CatalogAPI.Data.Features.Games;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Common.Endpoints;

namespace Features.Games.GetTopRatedGames;

public class Endpoint : Endpoint<GetTopRatedGamesRequest, PaginatedResponse<GameDto>>
{
    public CatalogContext Context { get; set; }

    public override void Configure()
    {
        Get("/top");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTopRatedGamesRequest req, CancellationToken ct)
    {
        var games = await Context
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
