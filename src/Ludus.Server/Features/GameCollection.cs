using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features;

public class GameCollection
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public long GameId { get; set; }
    public GameStatus Status { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public int? Rating { get; set; }
    public string? Notes { get; set; }
}

public record GameCollectionPreviewDto(
    Guid Id,
    GameDTO Game,
    GameStatus Status,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    DateTimeOffset? UpdatedAt,
    int? Rating,
    string? Notes
);

public record GameCollectionDto(
    Guid Id,
    long GameId,
    GameStatus Status,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    DateTimeOffset? UpdatedAt,
    int? Rating,
    string? Notes
);
