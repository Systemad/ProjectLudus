using FastEndpoints;
using Marten.Pagination;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Microsoft.EntityFrameworkCore;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.Common.Lists;
using WebAPI.Features.DataAccess;

namespace Me.Lists.Get;

public class Endpoint : Endpoint<GetListRequest, GameListDto>
{
    public LudusContext _context { get; set; }
    public IGameStore Store { get; set; }

    public override void Configure()
    {
        Get("/{ListId}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(GetListRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var list = await _context
            .Lists.Include(l => l.Games)
            .FirstOrDefaultAsync(l => l.Id == req.ListId, ct);

        if (list is null)
        {
            ThrowError("List doesn't exist!");
        }

        var gameIds = list.Games.Select(i => i.GameId).ToList();

        HashSet<long> hypedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(
            _context,
            userId
        );
        HashSet<long> wishlistedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId);

        await using var session = Store.QuerySession();

        var games = await session
            .Query<Game>()
            .Where(g => gameIds.Contains(g.Id))
            .ToPagedListAsync(req.PageNumber, req.PageSize);

        var previews = GameDtoMapper.MapGamesToDto(games, wishlistedGames, hypedGames);

        var page = new PaginatedResponse<GameDto>(
            previews,
            games.TotalItemCount,
            games.PageCount,
            games.PageSize,
            games.PageNumber,
            games.IsLastPage
        );
        await SendOkAsync(
            new GameListDto
            {
                Id = list.Id,
                Name = list.Name,
                Public = list.Public,
                TotalItems = list.Games.Count,
                Page = page,
            }
        );
    }
}
