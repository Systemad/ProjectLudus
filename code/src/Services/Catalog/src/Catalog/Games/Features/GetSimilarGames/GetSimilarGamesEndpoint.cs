using Catalog.Data;
using Catalog.Games.Dtos;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Games.Features.GetSimilarGames;

public class GetSimilarGamesRequest
{
    public long GameId { get; set; }
}

public class GetSimilarGamesResponse
{
    public List<GameDto> SimilarGames { get; set; }
}


public class GetSimilarGamesEndpoint : Endpoint<GetSimilarGamesRequest, GetSimilarGamesResponse>
{
    public CatalogContext Context { get; set; }

    public override void Configure()
    {
        Get("/similar-games/{GameId}");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetSimilarGamesRequest req, CancellationToken ct)
    {
        // DO SELECT HERE!!!
        var similarGames = await Context
            .Games.Where(x => x.Id == req.GameId)
            .Select(s => s.SimilarGames)
            .ToListAsync(cancellationToken: ct);

        if (similarGames is null)
        {
            ThrowError("Game doesn't exist!");
        }

        // NOT NEEDED RIGHT? SINCE WE JOIN AND USE PROJECTION???
        /*
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
        */

        await Send.OkAsync(new GetSimilarGamesResponse { SimilarGames = new List<GameDto>() }, ct);
    }
}
