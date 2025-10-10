using CatalogAPI.Data.Entities;

namespace CatalogAPI.Data.Features.GameModes;

public class GameModeEntity
{
    public long Id { get; set; }
    public long CreatedAt { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public long UpdatedAt { get; set; }
    public string Url { get; set; } = null!;
    public virtual ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}