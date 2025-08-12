using FastEndpoints;
using Marten;
using Shared.Features;
using WebAPI.Features.Common.Games;

namespace Public.Games.GetGamesByIds;

public class Endpoint : Endpoint<GetGameByIdsRequest, GetGamesByIdsResponse>
{
    public IDocumentStore GameStore { get; set; }
    public IGameService GameService { get; set; }

    public override void Configure()
    {
        Post("/by-ids");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetGameByIdsRequest req, CancellationToken ct)
    {
        if (req.GameIds.Length == 0)
        {
            AddError(r => r.GameIds, "Game IDs cannot be empty");
            ThrowIfAnyErrors();
        }

        await using var session = GameStore.QuerySession();
        var games = await session.LoadManyAsync<InsertIGDBGame>(ct, req.GameIds);
        var hydratedGames = await Task.WhenAll(
            games.Select(g => GameService.HydrateGameDetailAsync(g))
        );
        await Send.OkAsync(new GetGamesByIdsResponse(hydratedGames.ToList()), ct);
    }
}