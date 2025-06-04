using System.Security.Claims;
using Ludus.Server.Features.Collection;
using Ludus.Server.Features.Common;
using Ludus.Server.Features.Games;
using Ludus.Server.Features.Games.Common.Services;
using Ludus.Shared.Features.Games;
using Marten;

namespace Ludus.Server.Features.Lists.Services;

public interface IUserListService
{
    Task<UserGameListDto> GetUserGameListDtoAsync(
        ClaimsPrincipal user,
        Guid listId,
        bool fetchPreview
    );

    Task<UserGameListDto?> AddGameToUserListAsync(ClaimsPrincipal user, Guid listId, long gameId);
}

public class UserListService : IUserListService
{
    private IDocumentStore UserStore { get; set; }
    private IGameStore GameDb { get; set; }
    private IGameService GameService { get; set; }

    public UserListService(IDocumentStore userStore, IGameStore gameDb)
    {
        UserStore = userStore;
        GameDb = gameDb;
    }

    public async Task<UserGameListDto> GetUserGameListDtoAsync(
        ClaimsPrincipal user,
        Guid listId,
        bool fetchPreview
    )
    {
        await using var session = UserStore.LightweightSession();
        await using var gameSession = GameDb.LightweightSession();

        var list = await session.LoadAsync<UserGameList>(listId);
        var gameIds = fetchPreview ? list.Games.Take(6).ToList() : list.Games;
        var games = await gameSession
            .Query<Game>()
            .Where(g => gameIds.Contains(g.Id))
            .ToListAsync();
        var items = await GameService.ConvertIntoGameDtoAsync(user, games);
        var preview = new UserGameListDto(list.Id, list.Name, list.Public, list.Games.Count, items);
        return preview;
    }

    public async Task<UserGameListDto?> AddGameToUserListAsync(
        ClaimsPrincipal user,
        Guid listId,
        long gameId
    )
    {
        await using var session = UserStore.LightweightSession();
        await using var gameSession = GameDb.LightweightSession();

        var userId = Guid.Parse(user.Identity.Name);

        var updateList = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == listId);

        if (updateList is null)
            return null;

        if (updateList.Games.Contains(gameId))
            return null;

        updateList.Games.Add(gameId);
        session.Store(updateList);
        await session.SaveChangesAsync();

        var list = await GetUserGameListDtoAsync(user, listId, false);
        return list;
    }

    public async Task<IEnumerable<UserGameListDto>> GetUserGameListsDtoAsync(
        ClaimsPrincipal user,
        bool fetchPreview
    )
    {
        await using var session = UserStore.LightweightSession();
        await using var gameSession = GameDb.LightweightSession();
        var userId = Guid.Parse(user.Identity.Name);

        var lists = await session
            .Query<UserGameList>()
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var listDtos = new List<UserGameListDto>();
        foreach (var item in lists)
        {
            var list = await GetUserGameListDtoAsync(user, item.Id, fetchPreview);
            listDtos.Add(list);
        }

        return listDtos;
    }
}
