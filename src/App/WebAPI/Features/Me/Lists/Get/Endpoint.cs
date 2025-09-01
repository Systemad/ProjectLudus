using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using MartenExt = Marten;
using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.DataAccess;

namespace Me.Lists.Get;

public class Endpoint : Endpoint<GetListRequest, GameListDto>
{
    public LudusContext DbContext { get; set; }
    public MartenExt.IDocumentStore Store { get; set; }

    public override void Configure()
    {
        Get("/{ListId}");
        Group<MeListsGroup>();
    }

    public override async Task HandleAsync(GetListRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var list = await DbContext.Lists.Include(l => l.Games).FirstOrDefaultAsync(l => l.Id == req.ListId, cancellationToken: ct);

        if (list is null)
        {
            ThrowError("List doesn't exist!");
        }

        var gameIds = list.Games.Select(i => i.GameId).ToList();

        await using var session = Store.QuerySession();

        var platformDict = new Dictionary<long, Platform>();

        var games = await session
            .Query<IGDBGameFlat>()
            .Include(platformDict).On(x => x.Platforms)
            .Where(g => gameIds.Contains(g.Id))
            .ToListAsync(ct);
        var gameDtos = games.Select(x => x.MapToGameDto(platformDict));
        var gameDict = gameDtos.ToDictionary(x => x.Id, x => x);
        
        var listItemDtos = list.Games.Select(item => item.MapToGameListItemDto(gameDict[item.GameId]))
            .ToList();

        await Send.OkAsync(list.MapToGameListDto(listItemDtos));
    }
}