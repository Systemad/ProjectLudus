using CatalogAPI.Data.Features.Games;
using NodaTime;

namespace CatalogAPI.Data.Features.Genres;

public class GenreEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Instant UpdatedAt { get; set; }

    public ICollection<GameEntity> Genres { get; set; } = new List<GameEntity>();
}