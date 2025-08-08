using FastEndpoints;
using Marten;
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

    public override async Task HandleAsync(CancellationToken ct)
    {
        await using var session = GameStore.QuerySession();
        var genres = await session
            .Query<Genre>()
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(token: ct);
        var platforms = await session
            .Query<Platform>()
            .Select(x => new FilterItem(x.Id, x.Name))
            .ToListAsync(token: ct);
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
