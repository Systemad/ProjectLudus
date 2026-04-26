using System.ComponentModel.DataAnnotations;

namespace PlayAPI.Dtos;

public class GameMetricDto
{
    [Required]
    public long GameId { get; set; }

    [Required]
    public Guid SessionId { get; set; }

    [Required]
    public DateTime FirstVisitedAt { get; set; }

    [Required]
    public DateTime LastVisitedAt { get; set; }

    [Required]
    public int ViewCount { get; set; }
}
