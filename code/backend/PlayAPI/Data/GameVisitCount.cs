using Microsoft.EntityFrameworkCore;

namespace PlayAPI.Data;

[Index(nameof(GameId), IsUnique = true)]
public class GameVisitCount
{
    public int Id { get; set; }
    public long GameId { get; set; }
    public long Count { get; set; }
    public DateTime LastVisitedAt { get; set; }
}
