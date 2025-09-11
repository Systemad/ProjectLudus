using Microsoft.EntityFrameworkCore;
using SyncService.Data;
using SyncService.Features.Games;

namespace SyncService.Workers;

public class SyncWorker(
    IServiceProvider serviceProvider,
    ILogger<SyncWorker> logger) : IHostedService
{
    //private readonly IServiceProvider _serviceProvider = serviceProvider;
    //private readonly ILogger<SyncWorker> _logger = logger;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            //var startTime = DateTimeOffset.UtcNow;
            Log.StartingSyncWorker(logger, DateTimeOffset.UtcNow);
            //logger.LogInformation("Starting SyncWorker at {Time}", startTime);

            using var scope = serviceProvider.CreateScope();
            var gameSeedingOrch = scope.ServiceProvider.GetRequiredService<GameSeedingSeedingOrchestrator>();
            try
            {
                if (!await DatabaseHasDataAsync(scope))
                {
                    Log.InitialSeeding(logger);
                    //logger.LogInformation("Database empty: running initial seeding");
                    await gameSeedingOrch.RunInitialSeedingAsync();
                }
                else
                {
                    Log.CatchupSeeding(logger);
                    //logger.LogInformation("Database already seeded; running catchup seeding");
                    await gameSeedingOrch.RunCatchupSeedingAsync();
                }

                Log.DatabaseSeedSuccessful(logger, DateTimeOffset.UtcNow);
                //logger.LogInformation("Database seeding successful, {time}", DateTimeOffset.UtcNow);
            }
            catch (Exception e)
            {
                Log.DatabaseSeedingFailed(logger, DateTimeOffset.UtcNow, e.Message);
                //logger.LogError("Syncing database failed, {time}: {exception}", DateTimeOffset.UtcNow, e);
            }

            break;
        }
    }

    private static async Task<bool> DatabaseHasDataAsync(IServiceScope scope)
    {
        /*await using*/ var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await db.Games.AnyAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}