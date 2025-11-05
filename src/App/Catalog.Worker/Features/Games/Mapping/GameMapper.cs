using CatalogAPI.Data.Features.Games;
using Riok.Mapperly.Abstractions;

namespace Catalog.Worker.Features.Games.Mapping;

[Mapper]
public static partial class GameMapper
{
    [MapperIgnoreSource(nameof(GameEntity.Developers))]
    [MapperIgnoreSource(nameof(GameEntity.Publishers))]
    [MapperIgnoreSource(nameof(GameEntity.Platforms))]
    [MapperIgnoreSource(nameof(GameEntity.GameEngines))]
    [MapperIgnoreSource(nameof(GameEntity.GameModes))]
    [MapperIgnoreSource(nameof(GameEntity.Themes))]
    [MapperIgnoreSource(nameof(GameEntity.Genres))]
    public static partial void ApplyEntityUpdate(GameEntity update, GameEntity entity);
}
