using CatalogAPI.Seeding;
using FastEndpoints;
using IGDB;

namespace Admin.FetchResource;

public class FetchResourceRequest
{
    [FromHeader(HeaderName = "X-AdminKey", IsRequired = true)]
    public string AdminKey { get; set; }
}

public class FetchResourcesEndpoint : Endpoint<FetchResourceRequest, bool>
{
    public IDataFetcherService DataFetcherService { get; set; }

    public override void Configure()
    {
        Get("/filters");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FetchResourceRequest req, CancellationToken ct)
    {
        var validAdminKey =
            Config.GetValue<string>("ADMIN_KEY") is { } key && req.AdminKey == $"{key}";
        if (!validAdminKey)
        {
            await Send.UnauthorizedAsync(ct);
        }

        await DataFetcherService.FetchDataAsync(ct);
    }
}
