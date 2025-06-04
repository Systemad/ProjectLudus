using System.Security.Claims;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Games.Common.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> ConvertIntoGameDtoAsync(
        ClaimsPrincipal user,
        IEnumerable<Game> games
    );
}
