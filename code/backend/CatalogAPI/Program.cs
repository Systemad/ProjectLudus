using System.Text.Json;
using System.Text.Json.Serialization;
using CatalogAPI;
using CatalogAPI.Context;
using CatalogAPI.Data;
using CatalogAPI.Models;
using EFCore.ParadeDB.PgSearch;
using Jameak.CursorPagination;
using Jameak.CursorPagination.Enums;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Scalar.AspNetCore;
using Thinktecture;
using GameItem = CatalogAPI.GameItem;

var builder = WebApplication.CreateBuilder(args);

// Program.cs
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
            //np.u
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
});
var app = builder.Build();

app.MapDefaultEndpoints();

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
app.UseOutputCache();
var summaries = new[]
{
    "Freezing",
    "Bracing",
    "Chilly",
    "Cool",
    "Mild",
    "Warm",
    "Balmy",
    "Hot",
    "Sweltering",
    "Scorching",
};

app.MapGet(
        "api/weatherforecast",
        () =>
        {
            var forecast = Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        }
    )
    .WithName("GetWeatherForecast")
    .CacheOutput();

app.MapGet(
        "api/search",
        async (
            [AsParameters] GameSearchRequest req,
            HttpRequest request,
            CancellationToken token,
            AppDbContext dbContext,
            KeySetPaginationStrategy _keySetPaginationStrategy,
            OffsetPaginationStrategy _offsetPaginationStrategy
        ) =>
        {
            var hey = request;
            IQueryable<GamesSearch> query = dbContext.GamesSearches.AsQueryable();

            if (!string.IsNullOrWhiteSpace(req.Name))
                query = query.Where(g => EF.Functions.MatchDisjunction(g.Name, req.Name));

            if (req.Themes is { Length: > 0 })
            {
                foreach (var term in req.Themes)
                {
                    query = query.Where(p => EF.Functions.Term(p.Themes, term));
                }
            }

            if (req.Modes is { Length: > 0 })
            {
                foreach (var term in req.Modes)
                {
                    query = query.Where(p => EF.Functions.Term(p.Modes, term));
                }
            }
            if (req.Genres is { Length: > 0 })
            {
                foreach (var term in req.Genres)
                {
                    query = query.Where(p => EF.Functions.Term(p.Genres, term));
                }
            }
            var subQuery = query
                .Select(g => new
                {
                    g.Id,
                    g.Name,
                    g.Summary,
                    g.Storyline,
                    g.FirstReleaseDate,
                    g.GameType,
                    g.CoverUrl,
                    g.Themes,
                    g.Genres,
                    g.Modes,
                    g.ReleaseYear,
                })
                .AsSubQuery()
                .AsQueryable();

            var rows = subQuery
                .Select(sub => new GameSearchFacet
                {
                    Id = (long)sub.Id!,
                    Name = sub.Name,
                    Summary = sub.Summary,
                    Storyline = sub.Storyline,
                    FirstReleaseDate = sub.FirstReleaseDate,
                    GameType = sub.GameType,
                    CoverUrl = sub.CoverUrl,
                    ReleaseYear = sub.ReleaseYear,
                    TotalItems = EF.Functions.WindowFunction(
                        new WindowFunction<int>("COUNT", true)
                    ),
                    Score = EF.Functions.Score(sub.Id),
                    Themes = sub.Themes,
                    Genres = sub.Genres,
                    Modes = sub.Modes,
                    ThemeFacet = JsonSerializer.Deserialize<Facets>(
                        EF.Functions.WindowFunction(
                            new WindowFunction<string>("agg", false),
                            "{\"terms\":{\"field\": \"themes\"}}"
                        )
                    ),
                    GameModesFacets = JsonSerializer.Deserialize<Facets>(
                        EF.Functions.WindowFunction(
                            new WindowFunction<string>("agg", false),
                            "{\"terms\":{\"field\": \"modes\"}}"
                        )
                    ),
                    GenreFacet = JsonSerializer.Deserialize<Facets>(
                        EF.Functions.WindowFunction(
                            new WindowFunction<string>("agg", false),
                            "{\"terms\":{\"field\": \"genres\"}}"
                        )
                    ),
                    /*
                    GameTypeFacet = JsonSerializer.Deserialize<Facets>(
                        EF.Functions.WindowFunction(
                            new WindowFunction<string>("agg", false),
                            "{\"terms\":{\"field\": \"game_type\"}}"
                        )
                    ),
                    */
                })
                .OrderByDescending(x => x.Score);

            //
            //.Take(req.PageSize);

            var page = await KeySetPaginator.ApplyPaginationAsync<
                GameSearchFacet,
                KeySetPaginationStrategy.Cursor,
                KeySetPaginationStrategy
            >(
                _keySetPaginationStrategy,
                rows,
                DelegateMethods.ToListAsyncDelegate(),
                DelegateMethods.CountAsyncDelegate(),
                DelegateMethods.AnyAsyncDelegate(),
                req.AfterCursor,
                40,
                //paginationDirection: PaginationDirection.Forward,
                computeTotalCount: ComputeTotalCount.Never,
                computeNextPage: ComputeNextPage.EveryPageAndPreventNextPageQueryOnLastPage,
                cancellationToken: token
            );

            var pageMetadata = new PageMetadata
            {
                NextPageCursor =
                    page.NextCursor == null
                        ? null
                        : _keySetPaginationStrategy.CursorToString(page.NextCursor),
                HasNextPage = page.HasNextPage!.Value,
                HasPreviousPage = await page.HasPreviousPageAsync(),
                TotalCount = page.Items[0].Data.TotalItems ?? 0,
            };
            var data = page
                .Items.Select(item => new PagedItem<GameItem>()
                {
                    Cursor = _keySetPaginationStrategy.CursorToString(item.Cursor),
                    Item = item.Data.MapTo(),
                })
                .ToList();
            var themeFacet = page.Items.FirstOrDefault()?.Data.ThemeFacet;
            var gameModeFacet = page.Items.FirstOrDefault()?.Data.GameModesFacets;
            var genreFacet = page.Items.FirstOrDefault()?.Data.GenreFacet;
            //var gameTypeFacet = page.Items.FirstOrDefault()?.Data.GameTypeFacet;
            var facets = new List<Facets>([themeFacet, gameModeFacet, genreFacet]);
            return new PaginatedResponse<GameItem>(pageMetadata, data, new ItemFacets(facets));
        }
    )
    .WithName("Search");
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
