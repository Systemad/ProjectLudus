using Catalog.Games.Models;
using IGDB.Models;
using NodaTime;

namespace Catalog.Platforms.Models;

public class PlatformEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Abbreviation { get; set; }
    public long? Generation { get; set; }
    public string Slug { get; set; }
    public Instant UpdatedAt { get; set; }
    public Instant CreatedAt { get; set; }
    public PlatformLogo? Logo { get; set; }
    public ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}
