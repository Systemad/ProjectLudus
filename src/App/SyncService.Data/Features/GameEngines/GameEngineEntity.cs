using Shared.Features.Games;
using Shared.Features.IGDB;
using SyncService.Data.Entities;

namespace SyncService.Data.Features.GameEngines;

public partial class GameEngineEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Url { get; set; } = null!;

    public GameEngineLogo? Logo { get; set; }

    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}
