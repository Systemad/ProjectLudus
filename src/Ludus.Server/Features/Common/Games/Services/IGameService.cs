using System.Security.Claims;
using Ludus.Server.Features.Common.Games.Models;

namespace Ludus.Server.Features.Common.Games.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> CreateGameDtoAsync(ClaimsPrincipal user, IEnumerable<long> gamesIds);
}
