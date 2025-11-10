using Catalog.Games.Dtos;
using Catalog.Games.Models;

namespace Catalog.Games.Features;

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
    public static partial GameDto ToDto(GameEntity entity);
}
