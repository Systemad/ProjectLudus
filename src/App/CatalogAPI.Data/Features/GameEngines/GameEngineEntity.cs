using CatalogAPI.Data.Features.Games;
using IGDB.Models;
using NodaTime;

namespace CatalogAPI.Data.Features.GameEngines;

public class GameEngineEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Url { get; set; }

    public GameEngine Metadata { get; set; } = null!;
    public GameEngineLogo? Logo { get; set; }
    public Instant UpdatedAt { get; set; }
    public ICollection<GameEntity> Games { get; set; } = [];
}
