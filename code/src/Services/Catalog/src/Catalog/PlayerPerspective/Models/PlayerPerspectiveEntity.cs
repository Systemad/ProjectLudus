using Catalog.Games.Models;
using NodaTime;

namespace Catalog.PlayerPerspective.Models;

public class PlayerPerspectiveEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Url { get; set; }
    public Instant UpdatedAt { get; set; }
    public Instant CreatedAt { get; set; }
    public ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}