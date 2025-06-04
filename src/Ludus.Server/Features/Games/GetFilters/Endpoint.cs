using FastEndpoints;
using Ludus.Server.Features.Common;
using Ludus.Server.Features.Games.Common;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Server.Features.Games.GetFilters;

public class Endpoint : EndpointWithoutRequest<GetFiltersResponse>
{
    public IGameStore GameStore { get; set; }

    public override void Configure()
    {
        Get("/filters");
        AllowAnonymous();
        Group<GamesGroupEndpoint>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await using var session = GameStore.QuerySession();
        var genres = await session
            .Query<Genre>()
            .Select(x => new GenreFilter(x.Id, x.Name))
            .ToListAsync(token: ct);
        var platforms = await session
            .Query<Platform>()
            .Select(x => new PlatformFilter(x.Id, x.Name))
            .ToListAsync(token: ct);
        var gameTypes = await session
            .Query<InternalGameType>()
            .Select(x => new GameTypeFilter(x.OriginalId, x.Type))
            .ToListAsync(token: ct);
        var themes = await session
            .Query<Theme>()
            .Select(x => new ThemeFilter(x.Id, x.Name))
            .ToListAsync(token: ct);
        var gameModes = await session
            .Query<GameMode>()
            .Select(x => new GameModeFilter(x.Id, x.Name))
            .ToListAsync(token: ct);
        var gameEngines = await session
            .Query<GameEngineFilter>()
            .Select(x => new GameEngineFilter(x.Id, x.Name))
            .ToListAsync(token: ct);
        var playerPerspective = await session
            .Query<PlayerPerspective>()
            .Select(x => new PlayerPerspectiveFilter(x.Id, x.Name))
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

        await SendAsync(response);
    }
}
