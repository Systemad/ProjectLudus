namespace Ludus.Server.Features.Common.Games.Models;

public class UserGameState
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
    public bool IsWishlisted { get; set; } = false;
    public bool IsFavorited { get; set; } = false;
}

public record UserGameStateDto(
    Guid Id,
    long GameId,
    GameStatus Status,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    DateTimeOffset? UpdatedAt,
    int? Rating,
    string? Notes,
    bool IsWishlisted = false,
    bool IsFavorited = false
);
