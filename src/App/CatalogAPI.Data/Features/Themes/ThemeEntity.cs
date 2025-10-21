using CatalogAPI.Data.Features.Games;
using NodaTime;

namespace CatalogAPI.Data.Features.Themes;

public class ThemeEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Url { get; set; }
    public Instant UpdatedAt { get; set; }
    public Instant CreatedAt { get; set; }
    public virtual ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}
