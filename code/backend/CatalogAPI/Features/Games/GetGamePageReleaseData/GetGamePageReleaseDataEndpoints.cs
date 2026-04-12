using System.Linq;
using CatalogAPI.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Games.GetGamePageReleaseData;

public static class GetGamePageReleaseDataEndpoints
{
    public record GetGamePageReleaseDataResponse(GamePageReleaseDataDto Data);

    public static IEndpointRouteBuilder MapGetGamePageReleaseDataEndpoints(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        var group = routeBuilder.MapGroup("/api/games").CacheOutput("DefaultCache");

        group
            .MapGet("/{gameId:long}/page-release-data", GetGamePageReleaseData)
            .Produces<GetGamePageReleaseDataResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return routeBuilder;
    }

    private static async Task<IResult> GetGamePageReleaseData(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var releaseData = await db
            .Games.AsNoTracking()
            .Where(g => g.Id == gameId)
            .Select(g => new GamePageReleaseDataDto
            {
                GameId = g.Id,
                GameName = g.Name,
                Releases = g
                    .ReleaseDates.Where(rd => rd.PlatformNavigation != null)
                    .Select(rd => new GameReleaseDto
                    {
                        PlatformName = rd.PlatformNavigation!.Name,
                        PlatformSlug = rd.PlatformNavigation!.Slug,
                        ReleaseDate = rd.Date,
                        Region = rd.ReleaseRegionNavigation!.Region,
                        Human = rd.Human,
                        Developers = g
                            .InvolvedCompanies.Where(ic =>
                                ic.Developer == true && ic.CompanyNavigation!.Name != null
                            )
                            .Select(ic => new CompanyInfoDto(
                                ic.CompanyNavigation!.Id,
                                ic.CompanyNavigation.Name,
                                ic.CompanyNavigation.Slug
                            ))
                            .ToList(),
                        Publishers = g
                            .InvolvedCompanies.Where(ic =>
                                ic.Publisher == true && ic.CompanyNavigation!.Name != null
                            )
                            .Select(ic => new CompanyInfoDto(
                                ic.CompanyNavigation!.Id,
                                ic.CompanyNavigation.Name,
                                ic.CompanyNavigation.Slug
                            ))
                            .ToList(),
                    })
                    .ToList(),
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (releaseData is null)
            return Results.NotFound();

        return Results.Ok(new GetGamePageReleaseDataResponse(releaseData));
    }
}
