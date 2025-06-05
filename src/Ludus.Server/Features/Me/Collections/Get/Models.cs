using Ludus.Server.Features.Common.Collection;
using Ludus.Server.Features.Common.Games.Models;

namespace Ludus.Server.Features.Me.Collections.Get;

public class GetGameStateRequest : IGameStateRequest
{
    public long GameId { get; set; }
}

public record GetGameStateResponse(UserGameStateDto GameState);
