using Notifications.Extensions;
using ServiceDefaults;
using TickerQ.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddReleaseAlerts();

var app = builder.Build();

app.UseTickerQ();
app.MapDefaultEndpoints();
app.Run();

namespace Notifications
{
    public partial class Program;
}
