using Catalog.Features.Companies.Get;
using Catalog.Features.Games.Common.Dtos;
using Catalog.Features.Games.Common.Projections;

namespace Catalog.Features.Companies;

internal sealed class CompanyService(AppDbContext db) : ICompanyService
{
    public async Task<CompanyOverviewDto?> GetOverviewAsync(long companyId, CancellationToken ct)
    {
        return await db
            .Companies.Where(c => c.Id == companyId)
            .Select(c => new CompanyOverviewDto(
                c.Id.ToString(),
                c.Name,
                c.Slug,
                c.Description,
                c.Url,
                c.StartDate,
                c.Country,
                c.LogoNavigation!.ImageId,
                c.LogoNavigation!.ImageId,
                c.Parent == null
                    ? null
                    : new ParentCompanyDto(c.Parent.Id.ToString(), c.Parent.Name, c.Parent.Slug),
                c.StatusNavigation!.Name
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<GameBrowseDto>> GetGamesAsync(long companyId, CancellationToken ct)
    {
        var gameIds = await db
            .InvolvedCompanies.Where(ic => ic.Company == companyId)
            .Select(ic => ic.Game)
            .ToListAsync(ct);

        return await db
            .Games.Where(g => gameIds.Contains(g.Id))
            .AsSplitQuery()
            .SelectGameBrowseDto()
            .ToListAsync(ct);
    }
}
