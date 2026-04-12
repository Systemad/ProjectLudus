using System.Linq;
using CatalogAPI.Context;
using CatalogAPI.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Games.GetById;

public static class GetByIdEndpoints
{
    public record GetGameDetailsByIdResponse(GameDetails Game);

    public static IEndpointRouteBuilder MapGetByIdEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/games").CacheOutput("DefaultCache");

        group
            .MapGet("/{gameId:long}/details", GetGameDetailsById)
            .Produces<GetGameDetailsByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return routeBuilder;
    }

    private static async Task<IResult> GetGameDetailsById(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var gameDetailsDto = await db
            .Games.AsNoTracking()
            .Where(g => g.Id == gameId)
            .Select(g => new GameDetails
            {
                Id = g.Id,
                Url = g.Url,
                InvolvedCompanies = g
                    .InvolvedCompanies.Where(ic =>
                        !string.IsNullOrEmpty(ic.CompanyNavigation!.Name)
                        && !string.IsNullOrEmpty(ic.CompanyNavigation!.Slug)
                    )
                    .OrderBy(ic => ic.CompanyNavigation!.Name)
                    .Select(ic => new InvolvedCompanyDto(
                        ic.CompanyNavigation!.Name!,
                        ic.CompanyNavigation!.Slug!,
                        ic.Publisher == true,
                        ic.Developer == true
                    ))
                    .ToList(),
                Themes = g
                    .Themes.Where(theme => !string.IsNullOrEmpty(theme.Name))
                    .Select(theme => theme.Name!)
                    .ToList(),
                GameModes = g
                    .GameModes.Where(gameMode => !string.IsNullOrEmpty(gameMode.Name))
                    .Select(gameMode => gameMode.Name!)
                    .ToList(),
                PlayerPerspectives = g
                    .PlayerPerspectives.Where(perspective =>
                        !string.IsNullOrEmpty(perspective.Name)
                    )
                    .Select(perspective => perspective.Name!)
                    .ToList(),
                Websites = g
                    .Websites.Select(website => new WebsiteDto(
                        website.Url,
                        website.TypeNavigation!.Type,
                        website.Url,
                        website.Trusted
                    ))
                    .ToList(),
                AlternativeNames = g
                    .AlternativeNames.Select(alternativeName => new AlternativeNameDto(
                        alternativeName.Id,
                        alternativeName.Name,
                        alternativeName.Comment
                    ))
                    .ToList(),
                GameEngines = g
                    .Platforms.SelectMany(platform => platform.GameEngines)
                    .GroupBy(gameEngine => new
                    {
                        gameEngine.Id,
                        gameEngine.Name,
                        ImageId = gameEngine.LogoNavigation!.ImageId,
                        gameEngine.Url,
                    })
                    .Select(group => new GameEnginesDto(
                        group.Key.Id,
                        group.Key.Name,
                        group.Key.ImageId,
                        group.Key.Url
                    ))
                    .ToList(),
                LanguageSupports = g
                    .GameLocalizations.Where(localization =>
                        !string.IsNullOrEmpty(localization.Name)
                    )
                    .Select(localization => new LanguageSupportsDto(
                        localization.Name!,
                        null,
                        localization.RegionNavigation!.Name
                    ))
                    .ToList(),
                Franchises = g
                    .Franchises.Where(franchise =>
                        !string.IsNullOrEmpty(franchise.Name)
                        && !string.IsNullOrEmpty(franchise.Slug)
                    )
                    .Select(franchise => new FranchiseDto(franchise.Name!, franchise.Slug!))
                    .ToList(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (gameDetailsDto is null)
            return Results.NotFound();

        return Results.Ok(new GetGameDetailsByIdResponse(gameDetailsDto));
    }
}
