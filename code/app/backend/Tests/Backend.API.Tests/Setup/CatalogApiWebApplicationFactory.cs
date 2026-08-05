using CatalogAPISDK;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using TUnit.AspNetCore;

namespace Backend.API.Tests.Setup;

public sealed class CatalogApiWebApplicationFactory : TestWebApplicationFactory<Program>
{
    public static ApiClient CreateApiClient(HttpClient httpClient)
    {
        var adapter = new HttpClientRequestAdapter(
            new AnonymousAuthenticationProvider(),
            httpClient: httpClient
        )
        {
            BaseUrl = httpClient.BaseAddress!.ToString().TrimEnd('/'),
        };

        return new ApiClient(adapter);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.UseEnvironment("IntegrationTest");
        builder.UseSetting(
            "ConnectionStrings:catalogdb",
            "Host=localhost;Port=5432;Database=catalog_tests;Username=test;Password=test"
        );
    }
}
