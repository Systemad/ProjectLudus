using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Common.Games.Models;

namespace WebAPI.Features.Common.Games.Mappers;

public class GameDtoMapper
{
    private HydrationCache _hydrationCache { get; set; }

    public GameDtoMapper(HydrationCache hydrationCache)
    {
        _hydrationCache = hydrationCache;
    }

    public IEnumerable<GameDto> MapGamesToDto(
        IEnumerable<InsertIGDBGame> games,
        HashSet<long> wishlistedSet,
        HashSet<long> hypedSet
    )
    {
        return games
            .Where(g => g != null)
            .Select(g => MapToGameDto(g, wishlistedSet.Contains(g.Id), hypedSet.Contains(g.Id)))
            .ToList();
    }


    public GameDto MapToGameDto(InsertIGDBGame game, bool isWishlisted, bool isHyped)
    {
        var list = new List<InvolvedCompany>();
        foreach (var company in game.InvolvedCompanies)
        {
            var involvedCompany = new InvolvedCompany
            {
                Id = 0,
                Company = await _hydrationCache.GetCompanyAsync(company.Company),
                Developer = company.Developer,
                Porting = company.Porting,
                Publisher = company.Publisher,
                Supporting = company.Supporting
            };
            list.Add(involvedCompany);
        }
        return new GameDto()
        {
            Id = game.Id,
            Name = game.Name,
            ArtworkImageId = game.Artworks?.FirstOrDefault()?.ImageId ?? "",
            CoverImageId = game.Cover?.ImageId ?? "",
            FirstReleaseDate = game.FirstReleaseDate,
            InvolvedCompanies = await _hydrationCache.game.InvolvedCompanies
            Platforms = _hydrationCache.GetPlatforms(game.Platforms),
            ReleaseDates =
                game.ReleaseDates?.Select(rd => DateTimeOffset.FromUnixTimeSeconds(rd.Date).DateTime).ToList() ??
                [],
            GameType = game.GameType,
            IsWishlisted = isWishlisted,
            IsHyped = isHyped,
        };
    }
}