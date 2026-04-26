using System.ComponentModel.DataAnnotations;
using PlayAPI.Data;

namespace PlayAPI.Dtos;

public class GameEventDto
{
    [Required]
    public long GameId { get; set; }

    [Required]
    public GameEventType EventType { get; set; }

    [Required]
    public Guid SessionId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}
