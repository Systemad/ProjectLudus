using Ludus.Server.Features.Common.Collection;
using Ludus.Server.Features.Common.Games;
using Ludus.Server.Features.Common.Games.Models;

namespace Ludus.Server.Features.Me.Collections.Update;

public class UpdateUserGameStateRequest : IGameStateRequest
{
    public Guid Id { get; set; }
    public long GameId { get; set; }
    public GameStatus Status { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int? Rating { get; init; }
    public string? Notes { get; init; }
    public bool isFavorited { get; init; }
    public bool isWishlisted { get; init; }
}
