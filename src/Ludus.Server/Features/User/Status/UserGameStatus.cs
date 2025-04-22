using Ludus.Server.Features.Games;

namespace Ludus.Server.Features.User.Status;

public class UserGameStatus
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public long GameId { get; set; }
    public GameStatus Status { get; set; }
}
