using FastEndpoints;
using Marten;
using Shared.Features.Games;

namespace Public.Games.GetGameById;

public class Endpoint : Endpoint<GetGameByIdRequest, GetGamesByIdResponse>
{
    public IDocumentStore GameStore { get; set; }

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
        var game = await session.LoadAsync<IGDBGame>(req.GameId, ct);
        if (game is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await SendOkAsync(new GetGamesByIdResponse(game), ct);
    }
}
