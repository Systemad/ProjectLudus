using Shared.Features;
using Shared.Features.Games;
using Shared.Features.References.Platform;

namespace Me.Hypes;

public static class HypedMapper
{
    public static HypedItem ToDto(this IGDBGameFlat game, Dictionary<long, Platform> platformDict, bool isHyped)
    {
        return new HypedItem()
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
            IsHyped = isHyped
        };
    }
}