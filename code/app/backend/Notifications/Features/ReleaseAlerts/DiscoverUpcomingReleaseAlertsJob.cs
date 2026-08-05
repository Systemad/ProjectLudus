using TickerQ.Utilities.Base;
using TickerQ.Utilities.Interfaces;

namespace Notifications.Features.ReleaseAlerts;

public sealed class DiscoverUpcomingReleaseAlertsJob(ReleaseAlertDiscovery discovery)
    : ITickerFunction
{
    public async Task ExecuteAsync(
        TickerFunctionContext context,
        CancellationToken cancellationToken
    )
    {
        await discovery.DiscoverAsync(cancellationToken);
    }
}
