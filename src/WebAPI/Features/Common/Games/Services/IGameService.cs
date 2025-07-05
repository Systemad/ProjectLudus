using System.Security.Claims;
using WebAPI.Features.Common.Games.Models;

namespace WebAPI.Features.Common.Games.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> CreateGameDtoAsync(ClaimsPrincipal user, IEnumerable<long> gamesIds);
}
