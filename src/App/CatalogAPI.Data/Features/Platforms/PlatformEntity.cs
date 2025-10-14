using CatalogAPI.Data.Entities;
using IGDB.Models;
using NodaTime;

namespace CatalogAPI.Data.Features.Platforms;

public class PlatformEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Abbreviation { get; set; }
    public long? Generation { get; set; }
    public PlatformLogo? PlatformLogo { get; set; }
    public string? Slug { get; set; }
    public Instant UpdatedAt { get; set; }
    public virtual ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}
