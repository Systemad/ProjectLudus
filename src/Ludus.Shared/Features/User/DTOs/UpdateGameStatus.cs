using Ludus.Shared.Features.Games;

namespace Ludus.Shared.Features.User.DTOs;

public class UpdateGameStatus
{
    public long GameId { get; set; }
    public GameStatus GameStatus { get; set; }
}
