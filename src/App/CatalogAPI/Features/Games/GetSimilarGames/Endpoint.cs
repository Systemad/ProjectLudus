using FastEndpoints;
using Marten;
using Shared.Features;
using Shared.Features.Games;
using Shared.Features.References.Platform;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;

namespace Features.Games.GetSimilarGames;

public class Endpoint : Endpoint<GetSimilarGamesRequest, GetSimilarGamesResponse>
{
    public IDocumentStore GameStore { get; set; }

    public override void Configure()
    {
        Get("/similar-games/{GameId}");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetSimilarGamesRequest req, CancellationToken ct)
    {
        await using var session = GameStore.QuerySession();
        var similarGames = await session.Query<IGDBGameFlat>()
            .Where(x => x.Id == req.GameId)
            .Select(s => s.SimilarGames)
            .FirstOrDefaultAsync(token: ct);
        if (similarGames is null)
        {
            ThrowError("Game doesn't exist!");
        }

        var response = new List<GameDto>();
        if (similarGames.Count > 0)
        {
            var platformDict = new Dictionary<long, Platform>();

            var simGames = await session
                .Query<IGDBGameFlat>()
                .Include(platformDict).On(x => x.Platforms)
                .Where(x => x.Id.IsOneOf(similarGames))
                .ToListAsync(token: ct);

            var prev = simGames.Select(item =>
                    item.MapToGameDto(
                        platformDict))
                .ToList();

            response = prev.ToList();
        }

        await Send.OkAsync(new GetSimilarGamesResponse { SimilarGames = response }, ct);
    }
}