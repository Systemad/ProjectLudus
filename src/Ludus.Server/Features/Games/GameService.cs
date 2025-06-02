using System.Security.Claims;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Server.Features.Games;

public class GameService : IGameService
{
    private IDocumentStore _db;
    private IGameStore _store;

    public GameService(IDocumentStore db, IGameStore store)
    {
        _db = db;
        _store = store;
    }

    public async Task<IEnumerable<GameDto>> GetGameDtosAsync(
        ClaimsPrincipal user,
        IEnumerable<Game> games
    )
    {
        await using var session = _store.QuerySession();
        await using var userSession = _db.QuerySession();

        var gamesList = games.ToList();
        var previews = gamesList.Select(game => game.ToDto()).ToList();
        if (user.Identity?.IsAuthenticated == true)
        {
            var userId = Guid.Parse(user.Identity.Name);

            var gamesId = gamesList.Select(x => x.Id).ToArray();

            var collections = await userSession
                .Query<UserGameData>()
                .Where(x => x.UserId == userId && gamesId.Contains(x.GameId))
                .ToListAsync();
            var collectionDict = collections.ToDictionary(x => x.GameId, x => x);

            previews = GameDtoMapper.MapUserGameData(previews, collectionDict);
        }

        return previews;
    }
}
