using System.Linq.Expressions;
using CatalogAPI.Extensions;

namespace CatalogAPI.Features.Games.Common.Projections;

public static class GameDtoProjection
{
    public static readonly Expression<Func<Game, GameDto>> AsGameDto = game => new GameDto
    {
        Id = game.Id,
        Name = game.Name ?? string.Empty,
        FirstReleaseDate = game.FirstReleaseDateUtc.ToDateOnly(),
        GameType = game.GameTypeNavigation!.Type,
        GameStatus = game.GameStatusNavigation!.Status,
        CoverUrl = game.CoverNavigation!.ImageId,
        Themes = game.Themes.Select(t => new Feature(t.Name!, t.Slug!)).ToList(),
        Genres = game.Genres.Select(g => new Feature(g.Name!, g.Slug!)).ToList(),
        GameModes = game.GameModes.Select(m => new Feature(m.Name!, m.Slug!)).ToList(),
        Platforms = game.Platforms.Select(p => new Feature(p.Name!, p.Slug!)).ToList(),
        Developers = game
            .InvolvedCompanies.Where(ic => ic.Developer == true)
            .Select(ic => new Feature(ic.CompanyNavigation!.Name!, ic.CompanyNavigation!.Slug!))
            .ToList(),
    };

    public static IQueryable<GameDto> SelectGameDto(this IQueryable<Game> query) =>
        query.Select(AsGameDto);
}
