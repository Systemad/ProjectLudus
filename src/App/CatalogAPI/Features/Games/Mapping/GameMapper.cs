using CatalogAPI.Data.Features.Games;
using Riok.Mapperly.Abstractions;

namespace CatalogAPI.Features.Games.Mapping;

[Mapper]
public static partial class GameMapper
{
    [MapperIgnoreSource(nameof(GameEntity.Companies))]
    [MapperIgnoreSource(nameof(GameEntity.Platforms))]
    [MapperIgnoreSource(nameof(GameEntity.GameEngines))]
    [MapperIgnoreSource(nameof(GameEntity.GameModes))]
    [MapperIgnoreSource(nameof(GameEntity.Themes))]
    [MapperIgnoreSource(nameof(GameEntity.Genres))]
    public static partial GameDto ToDto(GameEntity entity);

    [MapperIgnoreSource(nameof(GameEntity.Companies))]
    [MapperIgnoreSource(nameof(GameEntity.Platforms))]
    [MapperIgnoreSource(nameof(GameEntity.GameEngines))]
    [MapperIgnoreSource(nameof(GameEntity.GameModes))]
    [MapperIgnoreSource(nameof(GameEntity.Themes))]
    [MapperIgnoreSource(nameof(GameEntity.Genres))]
    public static partial void ApplyEntityUpdate(GameEntity update, GameEntity entity);
}
