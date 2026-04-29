using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Npgsql;
using PlayAPI.Context;
using PlayAPI.Data;
using PlayAPI.Extensions;
using PlayAPI.Features.Cookies;
using PlayAPI.Features.Games.Analytics;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddOpenApi(options => { });
builder.AddServiceDefaults();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    optionsBuilder
        .UseNpgsql(
            builder.Configuration.GetConnectionString("playdev"),
            np =>
            {
                np.CommandTimeout(30);
                np.UseNodaTime();
                np.ConfigureDataSource(ds =>
                {
                    ds.UseNodaTime();
                });
            }
        )
        .UseSeeding(
            (context, _) =>
            {
                var testMetric = context.Set<GameMetric>().FirstOrDefault(g => g.GameId == 1020L);
                if (testMetric == null)
                {
                    context
                        .Set<GameMetric>()
                        .Add(
                            new GameMetric
                            {
                                GameId = 1020L,
                                SessionId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                                FirstVisitedAt = SystemClock.Instance.GetCurrentInstant(),
                                LastVisitedAt = SystemClock.Instance.GetCurrentInstant(),
                                ViewCount = 1,
                            }
                        );
                    context.SaveChanges();
                }
            }
        )
        .UseAsyncSeeding(
            async (context, _, cancellationToken) =>
            {
                var testMetric = await context
                    .Set<GameMetric>()
                    .FirstOrDefaultAsync(g => g.GameId == 1020L, cancellationToken);
                if (testMetric == null)
                {
                    context
                        .Set<GameMetric>()
                        .Add(
                            new GameMetric
                            {
                                GameId = 1020L,
                                SessionId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                                FirstVisitedAt = SystemClock.Instance.GetCurrentInstant(),
                                LastVisitedAt = SystemClock.Instance.GetCurrentInstant(),
                                ViewCount = 1,
                            }
                        );
                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        );
    optionsBuilder.UseSnakeCaseNamingConvention();
});
builder.EnrichNpgsqlDbContext<AppDbContext>();

builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<ConsentFilter>();
builder.Services.AddControllers();

builder.Services.AddPlayApiRateLimiting();
builder.Services.AddGamesAnalyticsServices();

var app = builder.Build();

app.UseRateLimiter();
app.MapControllers();

app.MapDefaultEndpoints();
app.MapGamesAnalyticsEndpoints();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(
        "/docs",
        options =>
        {
            options.WithTitle("My API Documentation").ForceDarkMode();
            options.DisableAgent();
            options.DisableTelemetry();
        }
    );
}

app.UseHttpsRedirection();

app.Run();
