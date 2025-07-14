using FastEndpoints;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Microsoft.EntityFrameworkCore;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Me.Lists.GetAll;

public class GameListPreviewDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Public { get; set; }
    public int TotalItems { get; set; }
    public List<GameDto> PreviewItems { get; set; }
}

public class Endpoint : EndpointWithoutRequest<List<GameListPreviewDto>>
{
    public LudusContext _context { get; set; }
    public IGameStore Store { get; set; }

    public override void Configure()
    {
        Get("/{all}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var lists = await _context
            .Lists.Where(x => x.Id == userId)
            .Include(l => l.Games)
            .ToListAsync();

        var previewGameIds = lists
            .SelectMany(l => l.Games.OrderBy(i => i.AddedAt).Take(4).Select(i => i.GameId))
            .Distinct()
            .ToList();

        await using var session = Store.QuerySession();

        var previewGames = await session
            .Query<Game>()
            .Where(g => previewGameIds.Contains(g.Id))
            .ToListAsync(ct);

        HashSet<long> hypedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(
            _context,
            userId
        );
        HashSet<long> wishlistedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId);

        var gameDtoMap = GameDtoMapper
            .MapGamesToDto(previewGames, wishlistedGames, hypedGames)
            .ToDictionary(dto => dto.Id);

        var response = lists
            .Select(list =>
            {
                var previewIds = list
                    .Games.OrderBy(i => i.AddedAt)
                    .Take(4)
                    .Select(i => i.GameId)
                    .ToList();

                var previewItems = previewIds
                    .Where(id => gameDtoMap.ContainsKey(id))
                    .Select(id => gameDtoMap[id])
                    .ToList();

                return new GameListPreviewDto
                {
                    Id = list.Id,
                    Name = list.Name,
                    Public = list.Public,
                    TotalItems = list.Games.Count,
                    PreviewItems = previewItems,
                };
            })
            .ToList();

        await SendOkAsync(response);
    }
}
