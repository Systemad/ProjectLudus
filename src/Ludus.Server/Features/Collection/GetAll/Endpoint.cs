using FastEndpoints;
using Ludus.Server.Features.Common;
using Ludus.Server.Features.Games;
using Ludus.Server.Features.Games.Common.Services;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Pagination;

namespace Ludus.Server.Features.Collection.GetAll;

public class Endpoint : Endpoint<GetGameCollectionRequest, GetGameCollectionResponse>
{
    public GameService GameService { get; set; }
    public IGameStore GameDb { get; set; }
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Get("/{all}");
        Group<CollectionGroupEndpoint>();
    }

    public override async Task HandleAsync(GetGameCollectionRequest req, CancellationToken ct)
    {
        var userId = Guid.Parse(User.Identity.Name);
        await using var session = UserStore.QuerySession();
        await using var gameSession = GameDb.QuerySession();
        var gameCollection = await session
            .Query<UserGameState>()
            .Where(x => x.UserId == userId)
            .ToPagedListAsync(req.PageNumber, req.PageSize);

        var games = await gameSession.LoadManyAsync<Game>(gameCollection.Select(x => x.GameId));
        var previews = await GameService.ConvertIntoGameDtoAsync(User, games);

        var response = new GetGameCollectionResponse(
            previews,
            gameCollection.TotalItemCount,
            gameCollection.PageCount,
            gameCollection.PageNumber,
            gameCollection.IsLastPage
        );
        await SendAsync(response);
    }
}
