using Catalog.Ingester.Extensions;
using IGDB;
using BuildingBlocks.Twitch;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// TODO await npgmq.InitAsync(); //SOMEWHERE ELSE, AT STARTUP MAYBE?
// TODO: WHAT THIS PROJET DOES
// STRICTLY: HANDLES WEBHOOK SUBSCRIPTIONS, RECIVES DATA, AND PUTS THEM INTO A QUEUE!!!
// NOTHING ELSE!!!

builder.Services.AddOpenApi();

// TODO: connection string not needed?
builder.Services.AddNpgmq("");


// await npgmqClient.InitAsync();

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
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
