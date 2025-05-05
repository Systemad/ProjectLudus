using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features;

public class GameEntry
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public long GameId { get; set; }
    public GameStatus Status { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int? Rating { get; set; }
    public string? Notes { get; set; }
}

public record GameEntryPreviewDto(
    Guid Id,
    GameDTO Game,
    GameStatus Status,
    DateTime? StartDate,
    DateTime? EndDate,
    DateTime? UpdatedAt,
    int? Rating,
    string? Notes
);

public record GameEntryDto(
    Guid Id,
    long GameId,
    GameStatus Status,
    DateTime? StartDate,
    DateTime? EndDate,
    DateTime? UpdatedAt,
    int? Rating,
    string? Notes
);
