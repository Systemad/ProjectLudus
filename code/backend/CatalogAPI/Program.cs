using System.Text.Json.Serialization;
using CatalogAPI.Context;
using CatalogAPI.Features.Games;
using CatalogAPI.Features.PopularityTypes;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddOpenApi(options =>
{
    //options.AddSchemaTransformer<IntegerSchemaTransformer>();
    //options.AddSchemaTransformer<NumberSchemaTransformer>();

    //options.AddSchemaTransformer<RequiredSchemaTransformer>();
    //options.AddSchemaTransformer<RequiredPropertySchemaTransformer>();
});

builder.AddServiceDefaults();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
    options.SerializerOptions.MaxDepth = 256;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Tell OpenApi generator to report number fields as integers/floats only, not strings
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    optionsBuilder.UseNpgsql(
        builder.Configuration.GetConnectionString("catalogdev"),
        np =>
        {
            np.CommandTimeout(30);
            np.UseNodaTime();
            np.ConfigureDataSource(ds =>
            {
                ds.UseNodaTime();
                //ds.EnableDynamicJson();
            });
            np.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }
    );
    optionsBuilder.UseSnakeCaseNamingConvention();
});
builder.EnrichNpgsqlDbContext<AppDbContext>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromHours(1);
    options.AddPolicy(
        "DefaultCache",
        outputCachePolicyBuilder => outputCachePolicyBuilder.Expire(TimeSpan.FromHours(1))
    );
});

builder.Services.AddGamesServices();

var app = builder.Build();

app.MapDefaultEndpoints();

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
app.UseOutputCache();

app.UseGamesEndpoints();
app.UsePopularityEndpoints();

app.Run();
