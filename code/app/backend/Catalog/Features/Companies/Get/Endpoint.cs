namespace Catalog.Features.Companies.Get;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long companyId,
        ICompanyService companyService,
        CancellationToken cancellationToken
    )
    {
        var company = await companyService.GetOverviewAsync(companyId, cancellationToken);
        return company is null ? Results.NotFound() : Results.Ok(new GetCompanyResponse(company));
    }
}
