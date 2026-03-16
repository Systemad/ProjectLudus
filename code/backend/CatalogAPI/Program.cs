using System.Text.Json;
using System.Text.Json.Serialization;
using CatalogAPI;
using CatalogAPI.Context;
using CatalogAPI.Data;
using CatalogAPI.Features.Search;
using CatalogAPI.Features.Tags;
using CatalogAPI.Models;
using EFCore.ParadeDB.PgSearch;
using Jameak.CursorPagination;
using Jameak.CursorPagination.Enums;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Scalar.AspNetCore;
using Thinktecture;

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
});

builder.Services.AddScoped<KeySetPaginationStrategy>();
builder.Services.AddScoped<OffsetPaginationStrategy>();

// builder.AddNpgsqlDbContext
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    optionsBuilder.UseNpgsql(
        builder.Configuration.GetConnectionString("catalogdev"),
        np =>
        {
            np.UseNodaTime();
            np.ConfigureDataSource(ds =>
            {
                ds.UseNodaTime();
                //ds.EnableDynamicJson();
            });
            np.UsePgSearch();
            np.AddWindowFunctionsSupport();
        }
    );
    optionsBuilder.UseSnakeCaseNamingConvention();
});

builder.EnrichNpgsqlDbContext<AppDbContext>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("Expire15Min", outputCachePolicyBuilder => 
        outputCachePolicyBuilder.Expire(TimeSpan.FromMinutes(15)));
    //options.DefaultExpirationTimeSpan = TimeSpan.FromHours(1);
});
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

app.UseSearchEndpoints();
app.UseTagsEndpoints();

app.Run();
