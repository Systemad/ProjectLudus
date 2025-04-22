using Ludus.Server.Features.Games;

namespace Ludus.Server.Features.User.Status.Requests;

public class UpdateGameStatusRequest
{
    public long GameId { get; set; }
    public GameStatus GameStatus { get; set; }
}
