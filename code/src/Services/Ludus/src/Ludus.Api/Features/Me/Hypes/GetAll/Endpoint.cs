using FastEndpoints;
using Me.Hypes;
using Me.Hypes.GetAll;
using Me.Hypes.Helpers;
using Shared.Features.References.Platform;
using Ludus.Api.Features.Auth.Extensions;
using Ludus.Api.Features.Common.Endpoints;
using Ludus.Api.Features.DataAccess;

namespace Me.Hyped.GetAll;

public class Endpoint : Endpoint<GetHypesGamesRequest, PaginatedResponse<HypedItem>>
{
    public LudusContext _context { get; set; }
    public IDocumentStore Store { get; set; }
    public override void Configure()
    {
        Get("/all");
        Group<MeHypesGroup>();
    }

    public override async Task HandleAsync(GetHypesGamesRequest req, CancellationToken ct)
    {
        await using var session = Store.QuerySession();

        var userId = User.GetUserId();

        HashSet<long> hypedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId, ct);

        var platformDict = new Dictionary<long, Platform>();
        var games = await session
            .Query<IGDBGameFlat>()
            .Include(platformDict).On(x => x.Platforms)
            .Where(g => g.Id.IsOneOf(hypedGames.ToArray()))
            .ToPagedListAsync(req.PageNumber, req.PageSize, token: ct);

        var previews = games.Select(item =>
                item.ToDto(
                    platformDict,
                    hypedGames.Contains(item.Id)))
            .ToList();

        await Send.OkAsync(
            new PaginatedResponse<HypedItem>(
                previews,
                games.TotalItemCount,
                games.PageCount,
                games.PageSize,
                games.PageNumber,
                games.IsLastPage
            ),
            cancellation: ct
        );
    }
}
