using CatalogAPI.Data.Entities;

namespace CatalogAPI.Data.Features.Genres;

public class GenreEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Url { get; set; } = null!;
    public long UpdatedAt { get; set; }

    public virtual ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}