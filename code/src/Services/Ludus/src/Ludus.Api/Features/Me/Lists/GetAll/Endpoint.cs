using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Shared.Features;
using Shared.Features.Games;
using Shared.Features.References.Platform;
using Ludus.Api.Features.Auth.Extensions;
using Ludus.Api.Features.Common.Games.Mappers;
using Ludus.Api.Features.DataAccess;
using MartenExt = Marten;

namespace Me.Lists.GetAll;

public class Endpoint : EndpointWithoutRequest<List<GameListDto>>
{
    public LudusContext _context { get; set; }
    public MartenExt.IDocumentStore Store { get; set; }

    public override void Configure()
    {
        Get("/{all}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var listsWithGames = await _context
            .Lists
            .Where(x => x.Id == userId)
            .Include(l => l.Games)
            .ToListAsync(cancellationToken: ct);

        var previewGameIds = listsWithGames
            .SelectMany(
                l => l.Games.
                    OrderBy(i => i.AddedAt)
                    .Take(4)
                    .Select(i => i.GameId))
            .Distinct()
            .ToList();

        await using var session = Store.QuerySession();
        var platformDict = new Dictionary<long, Platform>();
        
        var previewGames = await session
            .Query<IGDBGameFlat>()
            .Include(platformDict).On(x => x.Platforms)
            .Where(g => previewGameIds.Contains(g.Id))
            .ToListAsync(ct);

        var gamesDict = previewGames
            .Select(g => g.MapToGameDto(platformDict))
            .ToDictionary(g => g.Id);

        var response = listsWithGames
            .Select(list =>
            {
                var items = list.Games
                    .OrderBy(g => g.AddedAt)
                    .Where(g => gamesDict.ContainsKey(g.GameId))
                    .Select(g => g.MapToGameListItemDto(gamesDict[g.GameId]))
                    .ToList();

                return list.MapToGameListDto(items);
            })
            .ToList();

        await Send.OkAsync(response, ct);
    }
}