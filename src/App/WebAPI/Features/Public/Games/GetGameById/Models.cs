using Shared.Features;

namespace Public.Games.GetGameById;

public class GetGameByIdRequest
{
    public long GameId { get; set; }
}

public record GetGamesByIdResponse(IGDBGame Game);
