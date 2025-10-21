using CatalogAPI.Data.Features.Games;
using NodaTime;

namespace CatalogAPI.Data.Features.GameModes;

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