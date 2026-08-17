namespace Catalog.Features.Companies.Get;

public static class Endpoint
{
    public static async Task<Results<BadRequest, NotFound, Ok<GetCompanyResponse>>> HandleAsync(
        string companyId,
        ICompanyService companyService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(companyId, out var parsedCompanyId))
            return TypedResults.BadRequest();

        var company = await companyService.GetOverviewAsync(parsedCompanyId, cancellationToken);
        return company is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(new GetCompanyResponse(company));
    }
}
