using Ludus.Server.Features.Collection.Common;
using Ludus.Server.Features.Shared;

namespace Ludus.Server.Features.Collection.Update;

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
