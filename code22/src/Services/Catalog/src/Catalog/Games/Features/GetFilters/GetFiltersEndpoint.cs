using BuildingBlocks.Twitch;
using Catalog.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Games.Features.GetFilters;

public class GetFiltersEndpoint : EndpointWithoutRequest<GetFiltersResponse>
{
    public CatalogDbContext DbDbContext { get; set; }

    public override void Configure()
    {
        Get("/filters");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var genres = await DbDbContext
            .Genres.Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(ct);

        var platforms = await DbDbContext
            .Platforms.Where(x => ConsolePriority.IDS.Contains(x.Id))
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(ct);

        // TOOD: URGENT add GameTypeEntity !!
        /*
        var gameTypes = await await DbContext.Games
            .Select(x => new FilterItem(x.OriginalId, x.Type))
            .ToListAsync(token: ct);
        */
        var themes = await DbDbContext
            .Themes.Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(ct);

        // TOOD: URGENT add GameModeEntity !!

        /*
        var gameModes = await DbContext.GameMode
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(token: ct);
        */
        var gameEngines = await DbDbContext
            .GameEngines.Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(ct);

        // TOOD: URGENT add playerPerspectiveEntity !!

        /*
        var playerPerspective = await DbContext.p
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(token: ct);
        */

        var response = new GetFiltersResponse(
            genres,
            platforms,
            new List<FilterItem>(),
            //gameTypes,
            themes,
            new List<FilterItem>(),
            //gameModes,
            gameEngines,
            new List<FilterItem>()
        //playerPerspective
        );

        await Send.OkAsync(response, cancellation: ct);
    }
}
