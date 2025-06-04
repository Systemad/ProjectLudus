using Ludus.Server.Features.Collection.Common;

namespace Ludus.Server.Features.Collection.Get;

public class GetGameStateRequest : IGameStateRequest
{
    public long GameId { get; set; }
}

public record GetGameStateResponse(UserGameStateDto GameState);
