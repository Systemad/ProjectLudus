using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using PlayAPI.Context;
using PlayAPISDK;
using TUnit.AspNetCore;
using TUnit.Core.Interfaces;

namespace PlayAPI.Tests.Setup;

public class PlayApiWebApplicationFactory : TestWebApplicationFactory<Program>, IAsyncInitializer
{
    [ClassDataSource<DatabaseContainer>(Shared = SharedType.PerTestSession)]
    public DatabaseContainer Database { get; init; } = null!;

    public async Task InitializeAsync()
    {
        _ = Server;

        using var scope = Server.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.UseSetting(
            "ConnectionStrings:sandbox-db",
            Database.Container.GetConnectionString()
        );
        builder.ConfigureServices(services =>
        {
            //services.AddAuthentication(defaultScheme: "Test")
            //    .AddScheme<AuthenticationSchemeOptions, CustomerApiAuthenticationHandler>("Test", options => { });

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    Database.Container.GetConnectionString(),
                    x => x.MigrationsAssembly(typeof(Program).Assembly)
                )
            );

            /*
            // Remove OpenFeature's lifecycle service to prevent teardown errors
            // when the static Api singleton is shut down multiple times across tests.
            var openFeatureLifecycle = services.FirstOrDefault(
                d => d.ImplementationType == typeof(HostedFeatureLifecycleService));
            if (openFeatureLifecycle is not null)
            {
                services.Remove(openFeatureLifecycle);
            }
            */
        });

        builder.UseEnvironment("IntegrationTest");
    }

    public static ApiClient CreateApiClient(HttpClient httpClient)
    {
        var authProvider = new AnonymousAuthenticationProvider();
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);
        return new ApiClient(adapter);
    }
}
