using FastEndpoints;
using Marten;

namespace Ludus.Server.Features.Collection.Get;

public class Endpoint : Endpoint<GetGameStateRequest, GetGameStateResponse>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Get("/{GameId}");
        Group<CollectionGroupEndpoint>();
        //AllowAnonymous();
    }

    public override async Task HandleAsync(GetGameStateRequest r, CancellationToken c)
    {
        var userId = Guid.Parse(User.Identity.Name);

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
