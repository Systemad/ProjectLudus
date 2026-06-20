using CatalogAPI.Features.Companies.Get;
using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.Companies;

public interface ICompanyService
{
    Task<CompanyOverviewDto?> GetOverviewAsync(long companyId, CancellationToken ct);
    Task<List<GameBrowseDto>> GetGamesAsync(long companyId, CancellationToken ct);
}
