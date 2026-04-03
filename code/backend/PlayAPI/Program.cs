using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PlayAPI.Context;
using PlayAPI.Extensions;
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
    optionsBuilder.UseNpgsql(
        builder.Configuration.GetConnectionString("playdev"),
        np =>
        {
            np.CommandTimeout(30);
            np.UseNodaTime();
            np.ConfigureDataSource(ds =>
            {
                ds.UseNodaTime();
                //ds.EnableDynamicJson();
            });
        }
    );
    optionsBuilder.UseSnakeCaseNamingConvention();
});
builder.EnrichNpgsqlDbContext<AppDbContext>();
builder.Services.AddPlayApiRateLimiting();
builder.Services.AddGameClickTrackingServices();
builder.Services.AddTypesenseServices(builder.Configuration);
var app = builder.Build();

app.UseRateLimiter();

app.MapDefaultEndpoints();
app.MapEndpoints();

// Configure the HTTP request pipeline.
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
