using CatalogAPI.Data;
using CatalogAPI.Seeding;
using FastEndpoints;

namespace Admin.FetchResource;

public class TransformRequest
{
    [FromHeader(HeaderName = "X-AdminKey", IsRequired = true)]
    public string AdminKey { get; set; }
}

public class TransformEndpoint : Endpoint<TransformRequest, bool>
{
    public IDataFetcherService DataFetcherService { get; set; }
    public CatalogContext CatalogContext { get; set; }

    public override void Configure()
    {
        Get("/filters");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TransformRequest req, CancellationToken ct)
    {
        var validAdminKey =
            Config.GetValue<string>("ADMIN_KEY") is { } key && req.AdminKey == $"{key}";
        if (validAdminKey is false)
        {
            await Send.UnauthorizedAsync(ct);
        }

        if (CatalogContext.Games.Any())
            AddError("Database is not empty, you must reset it first!");

        ThrowIfAnyErrors();

        await DataFetcherService.FetchDataAsync(ct);

        // TODO: DROP DATABASE
        // RECREATE DATABASE, ALTHOUGH, MAKE NOT?
    }
}
