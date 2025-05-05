using Ludus.Shared.Features.Games;

namespace Ludus.Server.Features.Games;

public static class GameDtoMapper
{
    public static GameDTO ToGameDto(this Game game) =>
        new()
        {
            Id = game.Id,
            Name = game.Name,
            ArtworkImageId = game.Artworks?.FirstOrDefault()?.ImageId,
            CoverImageId = game.Cover?.ImageId,
            FirstReleaseDate = game.FirstReleaseDate,
            Publisher = game.InvolvedCompanies?.FirstOrDefault(ic => ic.Publisher)?.Company?.Name,
            Platforms = game.Platforms?.Select(p => p.Name).ToList() ?? [],
            ReleaseDates =
                game.ReleaseDates?.Select(rd =>
                        DateTimeOffset.FromUnixTimeSeconds(rd.Date).DateTime
                    )
                    .ToList() ?? [],
            GameType = game.GameType.Type,
        };
}
