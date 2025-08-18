using FastEndpoints;
using Marten;
using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Common.Games;
using WebAPI.Features.Common.Games.Mappers;

namespace Public.Games.GetGameById;

public class Endpoint : Endpoint<GetGameByIdRequest, GetGamesByIdResponse>
{
    public IDocumentStore GameStore { get; set; }
    public IGameService GameService { get; set; }

    public override void Configure()
    {
        Get("/{GameId}");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetGameByIdRequest req, CancellationToken ct)
    {
        if (req.GameId == 0)
        {
            AddError(r => r.GameId, "Game IDs cannot be empty");
            ThrowIfAnyErrors();
        }

        await using var session = GameStore.QuerySession();

        var involvedCompaniesDict = new Dictionary<long, InvolvedCompany>();
        var gameEnginesDict = new Dictionary<long, GameEngine>();
        var gameModesDict = new Dictionary<long, GameMode>();
        var genresDict = new Dictionary<long, Genre>();
        var keywordsDict = new Dictionary<long, Keyword>();
        var playerPerspectivesDict = new Dictionary<long, PlayerPerspective>();
        var platformsDict = new Dictionary<long, Platform>();
        var themesDict = new Dictionary<long, Theme>();

        var game = await session.Query<IGDBGameFlat>()
            .Include(gameEnginesDict).On(x => x.GameEngines)
            .Include(gameModesDict).On(x => x.Genres)
            .Include(genresDict).On(x => x.Genres)
            .Include(keywordsDict).On(x => x.Keywords)
            .Include(playerPerspectivesDict).On(x => x.PlayerPerspectives)
            .Include(platformsDict).On(x => x.Platforms)
            .Include(themesDict).On(x => x.Themes)
            .FirstOrDefaultAsync(token: ct);


        if (game is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }


        var denorm = game.ToGameDto(
            gameEnginesDict.Values.ToList(),
            gameModesDict.Values.ToList(),
            genresDict.Values.ToList(),
            new List<InvolvedCompany>(), // TODO: fix
            keywordsDict.Values.ToList(),
            platformsDict.Values.ToList(),
            playerPerspectivesDict.Values.ToList(),
            themesDict.Values.ToList());
        var hydrated = await GameService.HydrateGameDetailAsync(game);

        await Send.OkAsync(new GetGamesByIdResponse(hydrated), ct);
    }
}