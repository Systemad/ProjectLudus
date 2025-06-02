using Ludus.Server.Features.Games;
using Ludus.Server.Features.Shared;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features;

public class UserGameData
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

public class GameDetailWithUserData
{
    public Game GameDetial { get; set; }
    public IEnumerable<GameDto> SimilarGames { get; set; }
}

public record UserGameDataDto(
    Guid Id,
    long GameId,
    GameStatus Status,
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    DateTimeOffset? UpdatedAt,
    int? Rating,
    string? Notes
);
