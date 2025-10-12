using CatalogAPI.Data.Entities;
using Shared.Features.Games;
using Shared.Features.IGDB;

namespace CatalogAPI.Data.Features.GameEngines;

public partial class GameEngineEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Url { get; set; } = null!;

    public IgdbGameEngineLogo? Logo { get; set; }

    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}
