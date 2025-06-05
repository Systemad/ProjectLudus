using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Games.Models;
using Marten;

namespace Ludus.Server.Features.Me.Collections.Get;

public class Endpoint : Endpoint<GetGameStateRequest, GetGameStateResponse>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Get("/{GameId}");
        Group<MeCollectionsGroup>();
        //AllowAnonymous();
    }

    public override async Task HandleAsync(GetGameStateRequest r, CancellationToken c)
    {
        var userId = User.GetUserId();

        await using var session = UserStore.QuerySession();
        var gameState = await session
            .Query<UserGameState>()
            .Where(g => g.GameId == r.GameId)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();

        if (gameState is null)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendOkAsync();
    }
}
