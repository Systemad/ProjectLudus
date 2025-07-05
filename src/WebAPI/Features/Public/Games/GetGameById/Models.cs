using Shared.Features.Games;

namespace Public.Games.GetGameById;

public class GetGameByIdRequest
{
    public long GameId { get; set; }
}

public record GetGamesByIdResponse(Game Game);
