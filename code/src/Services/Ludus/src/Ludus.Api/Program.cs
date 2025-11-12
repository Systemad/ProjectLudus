using BuildingBlocks.FastEndpoint;
using BuildingBlocks.OpenApi;
using FastEndpoints;
using Ludus;
using SteamWebAPI2.Utilities;
using Ludus.Extensions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabaseInfrastructure();
builder.AddAuthInfrastucture();
builder.AddCommonInfrastructure();


builder.Services.AddTransient(x => new SteamWebInterfaceFactory(
    builder.Configuration["SteamWebAPIKey"]
));


var app = builder.Build();

builder.AddFastEndpoints<LudusRoot>("Ludus API");

if (app.Environment.IsDevelopment())
{
    app.UseAspnetOpenApi();
}

//app.UseDefaultFiles();
//app.UseStaticFiles();
//app.MapStaticAssets();

app.UseDefaultExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();

// TODO: seperate into Extension, "UseAuthExtensions"
app.UseAuthInfrastucture();


//app.UseOutputCache();

//app.MapControllers();
app.UseStatusCodePages();

//app.MapFallbackToFile("/index.html");

app.Run();