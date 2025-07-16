using FastEndpoints;
using Marten;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common;
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
        var game = await session.Query<Game>().FirstOrDefaultAsync(g => g.Id == req.GameId);
        if (game is null)
        {
            ThrowError("Game doesn't exist!");
        }
        var response = new List<GameDto>();
        if (game.SimilarGames.Count > 0)
        {
            var simGames = await session
                .Query<Game>()
                .Where(x => x.Id.IsOneOf(game.SimilarGames.Select(s => s.Id).ToList()))
                .ToListAsync();

            HashSet<long> hypedGames = [];
            HashSet<long> wishlistedGames = [];

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();

                wishlistedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(_context, userId);
                hypedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId);
            }

            var previews = GameDtoMapper.MapGamesToDto(simGames, wishlistedGames, hypedGames);
            response = previews.ToList();
        }

        await SendOkAsync(new GetSimilarGamesResponse { SimilarGames = response });
    }
}
