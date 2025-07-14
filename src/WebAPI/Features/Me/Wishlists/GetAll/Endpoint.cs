using FastEndpoints;
using Marten.Pagination;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Me.Wishlists.GetAll;

public class Endpoint : Endpoint<GetWishlistedGamesRequest, PaginatedResponse<GameDto>>
{
    public LudusContext _context { get; set; }
    public IGameStore Store { get; set; }

    public override void Configure()
    {
        Get("/all");
        Group<MeWishlistGroup>();
    }

    public override async Task HandleAsync(GetWishlistedGamesRequest req, CancellationToken ct)
    {
        await using var session = Store.QuerySession();
        var userId = User.GetUserId();

        HashSet<long> wishlistedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(
            _context,
            userId
        );

        HashSet<long> hypedGames = await HypesHelper.GetHypedGamesByIdsAsync(
            _context,
            userId,
            wishlistedGames
        );

        var games = await session
            .Query<Game>()
            .Where(g => hypedGames.Contains(g.Id))
            .ToPagedListAsync(req.PageNumber, req.PageSize);

        var previews = GameDtoMapper.MapGamesToDto(games, wishlistedGames, hypedGames);

        await SendAsync(
            new PaginatedResponse<GameDto>(
                previews,
                games.TotalItemCount,
                games.PageCount,
                games.PageSize,
                games.PageNumber,
                games.IsLastPage
            )
        );
    }
}
