using Catalog.Games.Models;
using NodaTime;

namespace Catalog.GameModes.Models;

public class GameModeEntity
{
    public long Id { get; set; }
    public long CreatedAt { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public Instant UpdatedAt { get; set; }
    public string Url { get; set; }
    public ICollection<GameEntity> Games { get; set; } = [];
}