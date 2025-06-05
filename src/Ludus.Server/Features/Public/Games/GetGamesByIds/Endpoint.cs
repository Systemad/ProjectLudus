using FastEndpoints;
using Ludus.Server.Features.Common;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Public.Games.GetGamesByIds;

public class Endpoint : Endpoint<GetGameByIdsRequest, GetGamesByIdsResponse>
{
    public IGameStore GameStore { get; set; }

    public override void Configure()
    {
        Get();
        AllowAnonymous();
        Group<GamesGroupEndpoint>();
    }

    public override async Task HandleAsync(GetGameByIdsRequest req, CancellationToken ct)
    {
        if (req.GameIds.Length == 0)
        {
            AddError(r => r.GameIds, "Game IDs cannot be empty");
            ThrowIfAnyErrors();
        }

        await using var session = GameStore.QuerySession();
        var games = await session.LoadManyAsync<Game>(req.GameIds);
        await SendOkAsync(new GetGamesByIdsResponse(games.ToList()));
    }
}
