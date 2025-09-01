using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Common.Games.Models;

namespace WebAPI.Features.Common.Games.Mappers;

public static partial class GameMappingExtensions
{
    public static GameDto MapToGameDto(this IGDBGameFlat game, Dictionary<long, Platform> platformDict)
    {
        return new GameDto()
        {
            Id = game.Id,
            Name = game.Name,
            ArtworkImageId = game.Artworks?.FirstOrDefault()?.ImageId ?? "",
            CoverImageId = game.Cover?.ImageId ?? "",
            FirstReleaseDate = game.FirstReleaseDate,
            Platforms = game.Platforms.Where(platformDict.ContainsKey).Select(id => platformDict[id]).ToList(),
            ReleaseDates =
                game.ReleaseDates?.Select(rd => DateTimeOffset.FromUnixTimeSeconds(rd.Date).DateTime).ToList() ??
                [],
            GameType = game.GameType,
        };
    }
}
