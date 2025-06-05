using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Games.Models;
using Marten;

namespace Ludus.Server.Features.Me.Collections.Create;

public class Endpoint : Endpoint<TrackGameStateRequest, TrackGameStateResponse>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Post("/");
        Group<MeCollectionsGroup>();
        PreProcessor<CheckGamePreProcessor>();
    }

    public override async Task HandleAsync(TrackGameStateRequest req, CancellationToken ct)
    {
        await using var userSession = UserStore.LightweightSession();
        var userId = User.GetUserId();

        var gameState = await userSession
            .Query<UserGameState>()
            .FirstOrDefaultAsync(x => x.GameId == req.GameId && x.UserId == userId);

        if (gameState is not null)
            ThrowError("You are already tracking this game!");

        var state = ProcessorState<CheckGamePreProcessorState>();
        var newUserGame = new UserGameState
        {
            GameId = state.Game.Id,
            UserId = userId,
            UpdatedAt = DateTimeOffset.UtcNow,
            Status = GameStatus.None,
        };

        userSession.Store(newUserGame);
        await userSession.SaveChangesAsync();

        await SendOkAsync();
    }
}
