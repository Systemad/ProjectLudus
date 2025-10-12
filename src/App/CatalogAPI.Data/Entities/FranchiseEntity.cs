using Shared.Features.IGDB;

namespace CatalogAPI.Data.Entities;

public class FranchiseEntity
{
    public long Id { get; set; }
    public IgdbCompany Metadata { get; set; } = null!;
    public long UpdatedAt { get; set; }
    public DateTime Added { get; set; }
    public virtual ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}