using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Common.Games.Models;

namespace WebAPI.Features.Common.Games.Mappers;

public static class GamePreviewMapper
{
    public static GamePreviewDto ToGamePreviewDto(this IGDBGameFlat game, Dictionary<long, Platform> platformDict, bool isWishlisted,
        bool isHyped)
    {
        return new GamePreviewDto()
        {
            Id = game.Id,
            Name = game.Name,
            ArtworkImageId = game.Artworks?.FirstOrDefault()?.ImageId ?? "",
            CoverImageId = game.Cover?.ImageId ?? "",
            FirstReleaseDate = game.FirstReleaseDate,
            //InvolvedCompanies = await _hydrationCache.game.InvolvedCompanies
            Platforms = game.Platforms.Where(platformDict.ContainsKey).Select(id => platformDict[id]).ToList(),
            ReleaseDates =
                game.ReleaseDates?.Select(rd => DateTimeOffset.FromUnixTimeSeconds(rd.Date).DateTime).ToList() ??
                [],
            GameType = game.GameType,
            IsWishlisted = isWishlisted,
            IsHyped = isHyped,
        };
    }
}