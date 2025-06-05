using FastEndpoints;
using Ludus.Server.Features.Common;
using Ludus.Server.Features.Common.Games.Models;
using Ludus.Server.Features.Public.Games.Common;
using Ludus.Server.Features.Public.Games.Common.Services;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Server.Features.Public.Games.GetSimilarGames;

public class Endpoint : Endpoint<GetSimilarGamesRequest, GetSimilarGamesResponse>
{
    public IGameStore GameStore { get; set; }
    public IGameService GameService { get; set; }

    public override void Configure()
    {
        Get("/{{GameId}}");
        AllowAnonymous();
        Group<GamesGroupEndpoint>();
    }

    public override async Task HandleAsync(GetSimilarGamesRequest req, CancellationToken ct)
    {
        await using var session = GameStore.QuerySession();
        var game = await session.Query<Game>().FirstOrDefaultAsync(g => g.Id == req.GameId);
        if (game is null)
        {
            ThrowError("Game doesn't exist!");
        }
        var response = new List<GameDto>();
        if (game.SimilarGames.Count > 0)
        {
            var simGames = await session
                .Query<Game>()
                .Where(x => x.Id.IsOneOf(game.SimilarGames.Select(s => s.Id).ToList()))
                .ToListAsync();
            var dtos = await GameService.CreateGameDtoAsync(User, simGames);
            response = dtos.ToList();
        }

        await SendOkAsync(new GetSimilarGamesResponse(response));
    }
}
