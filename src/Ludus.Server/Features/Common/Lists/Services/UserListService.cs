using System.Security.Claims;
using Ludus.Server.Features.Common.Games.Services;
using Ludus.Server.Features.Common.Users.Models;

namespace Ludus.Server.Features.Common.Lists.Services;

public interface IUserListService
{
    Task<GameListDto> ToPreviewDtoAsync(GameList list, ClaimsPrincipal user);
}

public class UserListService : IUserListService
{
    private IGameService GameService { get; set; }

    public UserListService(IGameService gameService)
    {
        GameService = gameService;
    }

    public async Task<GameListDto> ToPreviewDtoAsync(GameList list, ClaimsPrincipal user)
    {
        var items = await GameService.CreateGameDtoAsync(user, list.Games.Select(x => x.GameId));
        return new GameListDto
        {
            Id = list.Id,
            Name = list.Name,
            Public = list.Public,
            TotalItems = list.Games.Count,
            Items = items,
        };
    }
}
