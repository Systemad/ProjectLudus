namespace Catalog.Worker.Logging;

public static partial class Log
{
    [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Begin database migration at: `{now}`")]
    public static partial void BeginMigration(ILogger logger, DateTime now);
}