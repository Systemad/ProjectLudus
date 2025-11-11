using BuildingBlocks.OpenApi;
using IGDB;
using BuildingBlocks.Twitch;
using Catalog.Extensions;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// TODO await npgmq.InitAsync(); //SOMEWHERE ELSE, AT STARTUP MAYBE?
builder.AddDatabaseInfrastructure();

// TODO: connection string not needed?
builder.Services.AddNpgmq("");



builder.Services.Configure<TwitchOptions>(builder.Configuration.GetSection("IGDB"));
builder.Logging.AddConsole();

var igdbClient = IGDBClient.CreateWithDefaults(
    Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
    Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
);
builder.Services.AddSingleton<IGDBClient>(igdbClient);


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseAspnetOpenApi();
}

app.UseHttpsRedirection();


app.Run();
