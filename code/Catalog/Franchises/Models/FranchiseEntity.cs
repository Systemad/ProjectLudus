using Catalog.Games.Models;
using IGDB.Models;
using NodaTime;

namespace Catalog.Franchises.Models;

public class FranchiseEntity
{
    public long Id { get; set; }
    public Franchise Metadata { get; set; } = null!;
    public Instant UpdatedAt { get; set; }
    public Instant Added { get; set; }
    public ICollection<GameEntity> Games { get; set; } = [];
}