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

    public GameHydrator GameHydrator { get; set; }
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
        var game = await session.LoadAsync<InsertIGDBGame>(req.GameId, ct);
        if (game is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var hydrated = GameHydrator.HydrateGameAsync(game);
        await Send.OkAsync(new GetGamesByIdResponse(hydrated), ct);
    }
}
