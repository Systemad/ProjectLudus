using Microsoft.EntityFrameworkCore;
using SyncService.Data;
using SyncService.Features;
using SyncService.Features.Games;
using SyncService.Features.Seeding;

namespace SyncService.Workers;

public class SyncWorker(IServiceProvider serviceProvider, ILogger<SyncWorker> logger)
    : IHostedService
{
    //private readonly IServiceProvider _serviceProvider = serviceProvider;
    //private readonly ILogger<SyncWorker> _logger = logger;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Log.StartingSyncWorker(logger, DateTimeOffset.UtcNow);
            using var scope = serviceProvider.CreateScope();
            var seedingController = scope.ServiceProvider.GetRequiredService<ISeederService>();
            try
            {
                if (!await DatabaseHasDataAsync(scope))
                {
                    Log.InitialSeeding(logger);
                    //await seedingController.RunInitialSeedingAsync();
                }
                else
                {
                    Log.CatchupSeeding(logger);
                    //await seedingController.RunCatchupSeedingAsync();
                }

                Log.DatabaseSeedSuccessful(logger, DateTimeOffset.UtcNow);
            }
            catch (Exception e)
            {
                Log.DatabaseSeedingFailed(logger, DateTimeOffset.UtcNow, e.Message);
            }

            break;
        }
    }

    private static async Task<bool> DatabaseHasDataAsync(IServiceScope scope)
    {
        /*await using*/var db = scope.ServiceProvider.GetRequiredService<SyncDbContext>();
        return await db.Games.AnyAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
