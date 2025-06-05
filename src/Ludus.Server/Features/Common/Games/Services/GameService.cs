using System.Security.Claims;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Games.Mappers;
using Ludus.Server.Features.Common.Games.Models;
using Ludus.Server.Features.Public.Games.Common.Services;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Server.Features.Common.Games.Services;

public class GameService : IGameService
{
    private IDocumentStore _db;
    private IGameStore _store;

    public GameService(IDocumentStore db, IGameStore store)
    {
        _db = db;
        _store = store;
    }

    public async Task<IEnumerable<GameDto>> CreateGameDtoAsync(
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
            var userId = user.GetUserId();

            var gamesId = gamesList.Select(x => x.Id).ToArray();

            var collections = await userSession
                .Query<UserGameState>()
                .Where(x => x.UserId == userId && gamesId.Contains(x.GameId))
                .ToListAsync();
            var collectionDict = collections.ToDictionary(x => x.GameId, x => x);

            previews = GameDtoMapper.MapUserGameData(previews, collectionDict);
        }

        return previews;
    }
}
