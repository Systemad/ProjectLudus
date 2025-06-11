using System.Security.Claims;
using Ludus.Server.Features.Common.Games.Models;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Common.Games.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> CreateGameDtoAsync(ClaimsPrincipal user, IEnumerable<long> gamesIds);
}
