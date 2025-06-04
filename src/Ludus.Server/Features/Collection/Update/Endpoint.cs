using FastEndpoints;
using Marten;

namespace Ludus.Server.Features.Collection.Update;

public class Endpoint : Endpoint<UpdateUserGameStateRequest, UserGameStateDto>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Post("/{GameId}");
        Group<CollectionGroupEndpoint>();
    }

    public override async Task HandleAsync(UpdateUserGameStateRequest req, CancellationToken ct)
    {
        var userId = Guid.Parse(User.Identity.Name);
        await using var userSession = UserStore.LightweightSession();

        var gameState = await userSession
            .Query<UserGameState>()
            .FirstOrDefaultAsync(x => x.GameId == req.GameId && x.UserId == userId);
        if (gameState is null)
            ThrowError("You are not tracking this game!");

        gameState.StartDate = req.StartDate;
        gameState.EndDate = req.EndDate;
        gameState.Status = req.Status;
        gameState.Rating = req.Rating;
        gameState.Notes = req.Notes;
        gameState.IsFavorited = req.isFavorited;
        gameState.IsWishlisted = req.isWishlisted;

        await userSession.SaveChangesAsync();
        await SendOkAsync();
    }
}
