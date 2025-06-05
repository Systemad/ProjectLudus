using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Collection;
using Ludus.Server.Features.Common.Games.Models;
using Marten;

namespace Ludus.Server.Features.Me.Collections.Update;

public class Endpoint : Endpoint<UpdateUserGameStateRequest, UserGameStateDto>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Post("/update/{GameId}");
        Group<MeCollectionsGroup>();
    }

    public override async Task HandleAsync(UpdateUserGameStateRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
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
