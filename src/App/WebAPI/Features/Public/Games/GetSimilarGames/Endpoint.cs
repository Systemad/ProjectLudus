using FastEndpoints;
using Marten;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Games;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Public.Games.GetSimilarGames;

public class Endpoint : Endpoint<GetSimilarGamesRequest, GetSimilarGamesResponse>
{
    public IDocumentStore GameStore { get; set; }
    public LudusContext _context { get; set; }
    public IGameService GameService { get; set; }

    public override void Configure()
    {
        Get("/similar-games/{GameId}");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetSimilarGamesRequest req, CancellationToken ct)
    {
        await using var session = GameStore.QuerySession();
        var game = await session.LoadAsync<InsertIgdbGame>(req.GameId, ct);
        if (game is null)
        {
            ThrowError("Game doesn't exist!");
        }
        var response = new List<GameDto>();
        if (game.SimilarGames.Count > 0)
        {
            var simGames = await session
                .Query<InsertIgdbGame>()
                .Where(x => x.Id.IsOneOf(game.SimilarGames))
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

            var previews = await GameService.HydrateGamesAsync(simGames, wishlistedGames, hypedGames);
            response = previews.ToList();
        }

        await Send.OkAsync(new GetSimilarGamesResponse { SimilarGames = response }, ct);
    }
}
