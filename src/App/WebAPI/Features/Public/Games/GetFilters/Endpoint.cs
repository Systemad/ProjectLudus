using FastEndpoints;
using Marten;
using Shared.Features;
using Shared.Features.Games;

namespace Public.Games.GetFilters;

public class Endpoint : EndpointWithoutRequest<GetFiltersResponse>
{
    public IDocumentStore GameStore { get; set; }

    public override void Configure()
    {
        Get("/filters");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }
// use fusion cache here
    public override async Task HandleAsync(CancellationToken ct)
    {
        await using var session = GameStore.QuerySession();
        var genres = await session
            .Query<Genre>()
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(token: ct);

        var platforms = await session.Query<Platform>()
            .Where(x => ConsolePriority.IDS.Contains(x.Id))
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(ct);
        //var platforms = await session.LoadManyAsync<Platform>(ct, ConsolePriority.IDS);
        //var platformFilter = platforms.Select(x => new FilterItem(x.Id, x.Name)).ToList();
        //.Query<Platform>()
        //.Where(x => x.Name != null && slugs.Any(slug => x.Name.Contains(slug, StringComparison.CurrentCultureIgnoreCase)))
        //.Select(x => new FilterItem(x.Id, x.Name))
        //.ToListAsync(ct);
        var gameTypes = await session
            .Query<InternalGameType>()
            .Select(x => new FilterItem(x.OriginalId, x.Type))
            .ToListAsync(token: ct);
        var themes = await session
            .Query<Theme>()
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(token: ct);
        var gameModes = await session
            .Query<GameMode>()
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(token: ct);
        var gameEngines = await session
            .Query<FilterItem>()
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(token: ct);
        var playerPerspective = await session
            .Query<PlayerPerspective>()
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(token: ct);

        var response = new GetFiltersResponse(
            genres,
            platforms,
            gameTypes,
            themes,
            gameModes,
            gameEngines,
            playerPerspective
        );

        await Send.OkAsync(response, cancellation: ct);
    }
}