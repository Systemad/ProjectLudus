using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Public.Games.GetGamesByIds;

public class GetGameByIdsRequest
{
    public long[] GameIds { get; set; }
}

public record GetGamesByIdsResponse(List<Game> Games);
