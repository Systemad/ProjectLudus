using Ludus.Shared.Features.Games;

namespace Ludus.Shared.Features.User.DTOs;

public class UpdateGameStatus
{
    public long GameId { get; set; }
    public Games.GameStatus GameStatus { get; set; }
}
