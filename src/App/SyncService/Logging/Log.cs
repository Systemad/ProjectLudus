namespace SyncService;

public static partial class Log
{
    [LoggerMessage(EventId = 101, Level = LogLevel.Information, Message = "Starting SyncWorker at {Time}")]
    public static partial void StartingSyncWorker(ILogger logger, DateTimeOffset time);
    
    [LoggerMessage(EventId = 102, Level = LogLevel.Information, Message = "Database empty: running initial seeding")]
    public static partial void InitialSeeding(ILogger logger);
    
    [LoggerMessage(EventId = 103, Level = LogLevel.Information, Message = "Database already seeded: running catchup seeding")]
    public static partial void CatchupSeeding(ILogger logger);
    
    [LoggerMessage(EventId = 104, Level = LogLevel.Information, Message = "Database seeding successful, {Time}")]
    public static partial void DatabaseSeedSuccessful(ILogger logger, DateTimeOffset time);

    [LoggerMessage(EventId = 105, Level = LogLevel.Error, Message = "Syncing database failed, {Time}: {ErrorMessage}")]
    public static partial void DatabaseSeedingFailed(ILogger logger, DateTimeOffset time, string errorMessage);
}