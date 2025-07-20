using Shared.Features.Games;

namespace Public.Games.GetGamesByIds;

public class GetGameByIdsRequest
{
    public long[] GameIds { get; set; }
}

public record GetGamesByIdsResponse(List<RawGame> Games);
