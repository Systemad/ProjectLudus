using CatalogAPI.Data;
using CatalogAPI.Features;
using CatalogAPI.Features.Games;
using CatalogAPI.Seeding;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Workers;

public class SyncWorker(IServiceProvider serviceProvider, ILogger<SyncWorker> logger)
    : IHostedService
{
    //private readonly IServiceProvider _serviceProvider = serviceProvider;
    //private readonly ILogger<SyncWorker> _logger = logger;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Logging.Log.StartingSyncWorker(logger, DateTimeOffset.UtcNow);
            using var scope = serviceProvider.CreateScope();
            var seedingController = scope.ServiceProvider.GetRequiredService<IDataFetcherService>();
            try
            {
                if (!await DatabaseHasDataAsync(scope))
                {
                    Logging.Log.InitialSeeding(logger);
                    //await seedingController.RunInitialSeedingAsync();
                }
                else
                {
                    Logging.Log.CatchupSeeding(logger);
                    //await seedingController.RunCatchupSeedingAsync();
                }

                Logging.Log.DatabaseSeedSuccessful(logger, DateTimeOffset.UtcNow);
            }
            catch (Exception e)
            {
                Logging.Log.DatabaseSeedingFailed(logger, DateTimeOffset.UtcNow, e.Message);
            }

            break;
        }
    }

    private static async Task<bool> DatabaseHasDataAsync(IServiceScope scope)
    {
        /*await using*/var db = scope.ServiceProvider.GetRequiredService<CatalogContext>();
        return await db.Games.AnyAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
