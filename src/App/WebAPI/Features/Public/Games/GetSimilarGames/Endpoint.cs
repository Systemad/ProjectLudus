using FastEndpoints;
using Marten;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Public.Games.GetSimilarGames;

public class Endpoint : Endpoint<GetSimilarGamesRequest, GetSimilarGamesResponse>
{
    public IDocumentStore GameStore { get; set; }
    public LudusContext _context { get; set; }

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

        var response = new List<GamePreviewDto>();
        if (similarGames.Count > 0)
        {
            var platformDict = new Dictionary<long, Platform>();
            
            var simGames = await session
                .Query<IGDBGameFlat>()
                .Include(platformDict).On(x => x.Platforms)
                .Where(x => x.Id.IsOneOf(similarGames))
                .ToListAsync(token: ct);

            HashSet<long> hypedGames = [];
            HashSet<long> wishlistedGames = [];

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userId = User.GetUserId();

                wishlistedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(
                    _context,
                    userId,
                    ct
                );
                hypedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId, ct);
            }

            var prev = simGames.Select(item =>
                    item.ToGamePreviewDto(
                        platformDict,
                        wishlistedGames.Contains(item.Id),
                        hypedGames.Contains(item.Id)))
                .ToList();
            
            response = prev.ToList();
        }

        await Send.OkAsync(new GetSimilarGamesResponse { SimilarGames = response }, ct);
    }
}