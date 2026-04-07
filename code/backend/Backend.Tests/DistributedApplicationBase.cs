using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Projects;
using Respawn;
using TUnit.Core.Interfaces;

namespace Backend.Tests;


public class DistributedApplicationBaseParallelLimit : IParallelLimit
{
    public int Limit => 1;
}

[ParallelLimiter<DistributedApplicationBaseParallelLimit>]
public abstract class DistributedApplicationBase
{
    [ClassDataSource<DistributedApplicationBaseFactory>(Shared = SharedType.PerTestSession)]
    public required DistributedApplicationBaseFactory TestBaseFactory { get; init; } = null!;

    [Before(Test)]
    public async Task BeforeAnyInheritedTests() => await TestBaseFactory.ResetAsync();

    protected DistributedApplication GetDistributedApplication() => TestBaseFactory.DistributedApplication;

    protected WebApplicationFactory<Program> GetWebApplication() => TestBaseFactory.WebApplication;

    protected T GetRequiredService<T>() where T : class =>
        TestBaseFactory.WebApplication.Services.CreateScope().ServiceProvider.GetRequiredService<T>();
}

public class DistributedApplicationBaseFactory : IAsyncInitializer, IAsyncDisposable
{
    public DistributedApplication DistributedApplication { get; private set; } = null!;
    public WebApplicationFactory<Program> WebApplication { get; private set; } = null!;
    private Respawner _respawner = null!;
    //private string _playdevConnectionString = null!;
    private NpgsqlConnection _npgsqlConnection = null!;

    public async ValueTask DisposeAsync()
    {
        await DistributedApplication.DisposeAsync();
        await WebApplication.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        // Build the entire distributed application - all containers, configuration settings, etc.
        var distributedBuilder = await DistributedApplicationTestingBuilder.CreateAsync<AppHost>();

        distributedBuilder.Environment.EnvironmentName = "Test";
        DistributedApplication = await distributedBuilder.BuildAsync();
        await DistributedApplication.StartAsync();

        var connectionString = (await DistributedApplication.GetConnectionStringAsync("DefaultConnection"))!;
        _npgsqlConnection = new NpgsqlConnection(connectionString: connectionString);

        // Add services for web/integration tests.
        WebApplication = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.UseSetting("ConnectionStrings:DefaultConnection", connectionString);
            });

        using var _ = WebApplication.Services.CreateScope();
        //var conn = new NpgsqlConnectionStringBuilder(_playdevConnectionString).Database;
        _respawner = await Respawner.CreateAsync(_npgsqlConnection, new()
        {
            TablesToIgnore = ["__EFMigrationsHistory"],
            DbAdapter = DbAdapter.Postgres
        });
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(_npgsqlConnection);
    }
}