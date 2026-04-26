namespace CatalogAPI.Features.Companies.Get;

public static class GetCompanyEndpoint
{
    public record Response(CompanyOverviewDto Company);

    public static async Task<IResult> HandleAsync(
        long companyId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var company = await db
            .Companies.Where(c => c.Id == companyId)
            .Select(c => new CompanyOverviewDto(
                c.Id,
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
                    : new ParentCompanyDto(c.Parent.Id, c.Parent.Name, c.Parent.Slug),
                c.StatusNavigation!.Name
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (company is null)
            return Results.NotFound();

        return Results.Ok(new Response(company));
    }
}
