namespace CatalogAPI.Features.Companies.Get;

public record ParentCompanyDto(long Id, string? Name, string? Slug);

public record CompanyOverviewDto(
    long Id,
    string? Name,
    string? Slug,
    string? Description,
    string? Url,
    long? StartDate,
    long? Country,
    string? LogoId,
    string? LogoImageId,
    ParentCompanyDto? ParentCompany,
    string? Status
);
