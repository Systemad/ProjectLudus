using System.Security.Claims;
using Ludus.Server.Features.Common.Games.Models;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Public.Games.Common.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> CreateGameDtoAsync(ClaimsPrincipal user, IEnumerable<Game> games);
}
