using Ludus.Server.Features.Common.Games.Models;
using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Common.Games.Mappers;

public static class GameDtoMapper
{
    public static GameDto ToDto(this Game game) =>
        new(
            game.Id,
            game.Name,
            game.Artworks?.FirstOrDefault()?.ImageId,
            game.Cover?.ImageId,
            game.FirstReleaseDate,
            game.InvolvedCompanies?.FirstOrDefault(ic => ic.Publisher)?.Company?.Name,
            game.Platforms?.Select(p => p.Name).ToList() ?? [],
            game.ReleaseDates?.Select(rd => DateTimeOffset.FromUnixTimeSeconds(rd.Date).DateTime)
                .ToList() ?? [],
            game.GameType.Type
        );

    public static List<GameDto> MapUserGameData(
        IEnumerable<GameDto> gamePreviews,
        Dictionary<long, UserGameState> collections
    )
    {
        return gamePreviews
            .Select(dto =>
            {
                if (collections.TryGetValue(dto.Id, out var collection))
                {
                    return dto with
                    {
                        IsWishlisted = collection.IsWishlisted,
                        IsFavorited = collection.IsFavorited,
                    };
                }

                return dto;
            })
            .ToList();
    }
}
