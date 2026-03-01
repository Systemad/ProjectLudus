using System.Text.Json;
using CatalogAPI;
using CatalogAPI.Context;
using CatalogAPI.Data;
using CatalogAPI.Models;
using EFCore.ParadeDB.PgSearch;
using EFCore.ParadeDB.PgSearch.Internals.Functions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Scalar.AspNetCore;
using Thinktecture;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// builder.AddNpgsqlDbContext
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    optionsBuilder.UseNpgsql(np =>
    {
        np.UseNodaTime();
        np.ConfigureDataSource(ds =>
        {
            ds.UseNodaTime();
            //ds.EnableDynamicJson();
        });
        np.UsePgSearch();
        //np.u
    });
    optionsBuilder.UseSnakeCaseNamingConvention();
});

builder.EnrichNpgsqlDbContext<AppDbContext>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
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
        }
    );
    //app.MapScalarApiReference();
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
        "/weatherforecast",
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
        "/api/games",
        (string? name) =>
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

app.Run();

app.MapPost(
        "api/search",
        async (string hello, AppDbContext dbContext) =>
        {
            var results1 = await dbContext
                .GamesSearches.Where(g => EF.Functions.MatchDisjunction(g.Name, "zombies"))
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
                .AsQueryable()
                .Select(sub => new GameSearchFacet
                {
                    Id = 0,
                    Name = sub.Name,
                    Summary = sub.Summary,
                    Storyline = sub.Storyline,
                    FirstReleaseDate = sub.FirstReleaseDate,
                    GameType = sub.GameType,
                    CoverUrl = sub.CoverUrl,
                    ReleaseYear = sub.ReleaseYear,
                    TotalItems = EF.Functions.WindowFunction(new WindowFunction<int>("COUNT", true)),
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
                    GenreFacet = JsonSerializer.Deserialize<Facets>(
                        EF.Functions.WindowFunction(
                            new WindowFunction<string>("agg", false),
                            "{\"terms\":{\"field\": \"genres\"}}"
                        )
                    ),
                })
                .OrderByDescending(x => x.Score)
                .Take(20)
                .ToListAsync();
           
        }
    )
    .WithName("Search");

app.MapGet(
        "api/searching",
        async ([AsParameters] GameSearchRequest req, AppDbContext dbContext) =>
        {
            IQueryable<GamesSearch> query = dbContext.GamesSearches.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(req.Name))
                query = query.Where(g => EF.Functions.MatchDisjunction(g.Name, req.Name));

            if (req.Genres.Length > 0)
            {
                foreach (var term in req.Genres)
                {
                    query = query.Where(p => EF.Functions.Term(p.Genres, term));
                }
            }

            if (req.Themes.Length > 0)
            {
                foreach (var term in req.Themes)
                {
                    query = query.Where(p => EF.Functions.Term(p.Themes, term));
                }
            }
            
            if (req.Modes.Length > 0)
            {
                foreach (var term in req.Modes)
                {
                    query = query.Where(p => EF.Functions.Term(p.Modes, term));
                }
            }
            
            var subQuery =  dbContext
                .GamesSearches.Where(g => EF.Functions.MatchDisjunction(g.Name, "zombies"))
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
                
                
               var rows = await subQuery.Select(sub => new GameSearchFacet
                {
                    Id = 0,
                    Name = sub.Name,
                    Summary = sub.Summary,
                    Storyline = sub.Storyline,
                    FirstReleaseDate = sub.FirstReleaseDate,
                    GameType = sub.GameType,
                    CoverUrl = sub.CoverUrl,
                    ReleaseYear = sub.ReleaseYear,
                    TotalItems = EF.Functions.WindowFunction(new WindowFunction<int>("COUNT", true)),
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
                    GenreFacet = JsonSerializer.Deserialize<Facets>(
                        EF.Functions.WindowFunction(
                            new WindowFunction<string>("agg", false),
                            "{\"terms\":{\"field\": \"genres\"}}"
                        )
                    ),
                })
                .OrderByDescending(x => x.Score)
                .Take(req.PageSize)
                .ToListAsync();
               
               var hasNextPage = rows.Count > req.PageSize;
               var data = hasNextPage ? rows.Take(req.PageSize).ToList() : rows;
               var nextCursor = data.LastOrDefault()?.Id;
               
               return Results.Ok(new
               {
                   data,
                   pagination = new
                   {
                       req.PageSize,
                       hasNextPage,
                       nextCursor,
                       totalItems = rows.FirstOrDefault()?.TotalItems ?? 0
                   }
               });
        }
    )
    .WithName("Searching");
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
