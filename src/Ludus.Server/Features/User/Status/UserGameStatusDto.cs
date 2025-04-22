using Ludus.Server.Features.Games;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.User.Status;

public class UserGameStatusDto
{
    public Guid Id { get; set; }

    //public Guid UserId { get; set; }
    public Game? Game { get; set; }
    public GameStatus Status { get; set; }
}
