using System.Security.Claims;
using Ludus.Server.Features.Public.Games.Common.Services;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Server.Features.Common.Lists.Services;

public interface IUserListService
{
    Task<UserGameListDto> ToPreviewDtoAsync(UserGameList list, ClaimsPrincipal user);
}

public class UserListService : IUserListService
{
    private IDocumentStore UserStore { get; set; }
    private IGameStore GameStore { get; set; }
    private IGameService GameService { get; set; }

    public UserListService(IDocumentStore userStore, IGameStore gameStore)
    {
        UserStore = userStore;
        GameStore = gameStore;
    }

    public async Task<UserGameListDto> ToPreviewDtoAsync(UserGameList list, ClaimsPrincipal user)
    {
        await using var gameSession = GameStore.LightweightSession();
        var gameIds = list.Games.Take(4).ToList();

        var games = await gameSession
            .Query<Game>()
            .Where(g => gameIds.Contains(g.Id))
            .ToListAsync();

        var items = await GameService.CreateGameDtoAsync(user, games);
        return new UserGameListDto(list.Id, list.Name, list.Public, list.Games.Count, items);
    }
}
