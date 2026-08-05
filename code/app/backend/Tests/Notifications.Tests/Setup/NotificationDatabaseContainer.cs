using Testcontainers.PostgreSql;
using TUnit.Core.Interfaces;

namespace Notifications.Tests.Setup;

public sealed class NotificationDatabaseContainer : IAsyncInitializer, IAsyncDisposable
{
    public PostgreSqlContainer Container { get; } = new PostgreSqlBuilder("postgres:17.6").Build();

    public Task InitializeAsync()
    {
        return Container.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await Container.DisposeAsync();
    }
}
