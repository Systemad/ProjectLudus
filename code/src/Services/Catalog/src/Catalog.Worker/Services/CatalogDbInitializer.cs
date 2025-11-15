using System.Diagnostics;
using Catalog.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Worker.Services;

public class CatalogDbInitializer(IServiceProvider serviceProvider, ILogger<CatalogDbInitializer> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    public async Task InitializeDatabaseAsync(CatalogDbContext dbDbContext, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        var strategy = dbDbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(dbDbContext.Database.MigrateAsync, cancellationToken);

        await SeedAsync();
    }

    private async Task SeedAsync()
    {
        throw new NotImplementedException();
    }
}