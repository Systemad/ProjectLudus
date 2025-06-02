using System.Security.Claims;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Games;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetGameDtosAsync(ClaimsPrincipal user, IEnumerable<Game> games);
}
