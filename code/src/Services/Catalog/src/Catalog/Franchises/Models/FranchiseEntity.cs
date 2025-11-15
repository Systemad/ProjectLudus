using Catalog.Games.Models;
using IGDB.Models;
using NodaTime;

namespace Catalog.Franchises.Models;

public class FranchiseEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Url { get; set; }
    public Instant UpdatedAt { get; set; }
    public Instant CreatedAt { get; set; }
    public Franchise Metadata { get; set; } = null!;
    public ICollection<GameEntity> Games { get; set; } = [];
}