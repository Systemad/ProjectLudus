namespace Catalog.Features.Companies.Get;

public record ParentCompanyDto(string Id, string? Name, string? Slug);

public record CompanyOverviewDto(
    string Id,
    string Name,
    string Slug,
    string? Description,
    string? Url,
    long? StartDate,
    long? Country,
    string? LogoId,
    string? LogoImageId,
    ParentCompanyDto? ParentCompany,
    string? Status
);
