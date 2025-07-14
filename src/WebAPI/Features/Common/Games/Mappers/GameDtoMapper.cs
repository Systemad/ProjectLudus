using Shared.Features.Games;
using WebAPI.Features.Common.Games.Models;

namespace WebAPI.Features.Common.Games.Mappers;

public static class GameDtoMapper
{
    public static IEnumerable<GameDto> MapGamesToDto(
        IEnumerable<Game> games,
        HashSet<long> wishlistedSet,
        HashSet<long> hypedSet
    )
    {
        var gameDtos = games
            .Select(g => new GameDto()
            {
                Id = g.Id,
                Name = g.Name,
                ArtworkImageId = g.Artworks.FirstOrDefault()?.ImageId,
                CoverImageId = g.Cover?.ImageId,
                FirstReleaseDate = g.FirstReleaseDate,
                Publisher = g.InvolvedCompanies.FirstOrDefault(ic => ic.Publisher)?.Company?.Name,
                Platforms = g.Platforms?.Select(p => p.Name).ToList(),
                ReleaseDates = g
                    .ReleaseDates?.Select(rd =>
                        DateTimeOffset.FromUnixTimeSeconds(rd.Date).DateTime
                    )
                    .ToList(),
                GameType = g.GameType?.Type,
                IsWishlisted = wishlistedSet.Contains(g.Id),
                IsHyped = hypedSet.Contains(g.Id),
            })
            .ToList();

        return gameDtos;
    }
}
