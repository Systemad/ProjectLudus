using Ludus.Shared.Features.Games;

namespace Public.Games.GetGamesById;

public class GetGameByIdRequest
{
    public long GameId { get; set; }
}

public record GetGamesByIdResponse(Game Game);
