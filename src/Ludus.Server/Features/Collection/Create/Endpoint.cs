using FastEndpoints;
using Ludus.Server.Features.Collection.Common;
using Ludus.Server.Features.Shared;
using Marten;

namespace Ludus.Server.Features.Collection.Create;

public class Endpoint : Endpoint<TrackGameStateRequest, TrackGameStateResponse>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Post("/{GameId}");
        Group<CollectionGroupEndpoint>();
        PreProcessor<CollectionPreProcessor>();
    }

    public override async Task HandleAsync(TrackGameStateRequest req, CancellationToken ct)
    {
        await using var userSession = UserStore.LightweightSession();
        var userId = Guid.Parse(User.Identity.Name);

        var gameState = await userSession
            .Query<UserGameState>()
            .FirstOrDefaultAsync(x => x.GameId == req.GameId && x.UserId == userId);

        if (gameState is not null)
            ThrowError("You are already tracking this game!");

        var state = ProcessorState<CollectionPreProcessorState>();
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
        //await SendAsync(new TrackGameResponse(newUserGame.AsDto()));
    }
}
