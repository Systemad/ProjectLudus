namespace CatalogAPI.Features.Games.Mappers;

[Mapper]
public static partial class GameSearchMapper
{
    public static partial List<GamesSearchDto> MapToDto(IEnumerable<GamesSearch> q);
}
