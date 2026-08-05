using Catalog.Features.Companies.Get;
using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Companies;

public interface ICompanyService
{
    Task<CompanyOverviewDto?> GetOverviewAsync(long companyId, CancellationToken ct);
    Task<List<GameBrowseDto>> GetGamesAsync(long companyId, CancellationToken ct);
}
