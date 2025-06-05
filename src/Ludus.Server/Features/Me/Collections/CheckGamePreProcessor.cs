using FastEndpoints;
using Ludus.Server.Features.Common;
using Ludus.Server.Features.Common.Collection;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Server.Features.Me.Collections;

public class CheckGamePreProcessorState
{
    public Game Game { get; set; }
}

public class CheckGamePreProcessor : PreProcessor<IGameStateRequest, CheckGamePreProcessorState>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<IGameStateRequest> context,
        CheckGamePreProcessorState state,
        CancellationToken ct
    )
    {
        if (context.Request is null)
        {
            return;
        }

        var gameStore = context.HttpContext.Resolve<IGameStore>();
        await using var session = gameStore.QuerySession();

        var game = await session
            .Query<Game>()
            .Where(g => g.Id == context.Request.GameId)
            .FirstOrDefaultAsync();
        if (game is null)
        {
            await context.HttpContext.Response.SendNotFoundAsync();
            return;
        }

        state.Game = game;
    }
}
