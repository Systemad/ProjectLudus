using CatalogAPI.Data.Features.Games;
using IGDB.Models;
using NodaTime;

namespace CatalogAPI.Data.Features.Franchises;

public class FranchiseEntity
{
    public long Id { get; set; }
    public Franchise Metadata { get; set; } = null!;
    public Instant UpdatedAt { get; set; }
    public Instant Added { get; set; }
    public ICollection<GameEntity> Games { get; set; } = [];
}