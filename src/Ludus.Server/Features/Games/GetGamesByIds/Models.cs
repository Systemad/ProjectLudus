using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Games.Get;

public class GetGameByIdsRequest
{
    public long[] GameIds { get; set; }
}

public record GetGamesByIdsResponse(List<Game> Games);
