using Ludus.Server.Configuration;
using Ludus.Server.Features.Auth;
using Ludus.Server.Features.Collection;
using Ludus.Server.Features.Collection.Services;
using Ludus.Server.Features.Exceptions;
using Ludus.Server.Features.Games;
using Ludus.Server.Features.Lists;
using Ludus.Server.Features.Lists.Services;
using Ludus.Server.Features.User;
using Marten;
using Scalar.AspNetCore;
using SteamWebAPI2.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMartenDatabases(builder.Environment, builder.Configuration);

builder.Services.AddAuth();

builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(30);
});
builder.Services.AddExceptionHandler<GlobalExceptionsHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddProblemDetails();
builder.Services.AddTransient(x => new SteamWebInterfaceFactory(
    builder.Configuration["SteamWebAPIKey"]
));

//builder.Services.ConfigureOpenTelemetry(builder.Configuration);

builder.Services.AddSingleton<GameCollectionService>();
builder.Services.AddSingleton<UserListService>();
var app = builder.Build();

/*
using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetRequiredService<IDocumentStore>();

    await store.Advanced.Clean.CompletelyRemoveAllAsync();
    await store.Advanced.Clean.DeleteAllDocumentsAsync();
    await store.Advanced.Clean.DeleteAllEventDataAsync();
}
*/
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.Servers = [];
        //    options.Authentication = new() { PreferredSecurityScheme = IdentityConstants.BearerScheme };
    });

    app.MapOpenApi();
    app.UseWebAssemblyDebugging();
    //app.Map("/", () => Results.Redirect("/scalar/v1"));
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.MapStaticAssets();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();

app.MapRazorPages();

// Endpoints
app.MapAuthEndpoints();

app.MapGameCollectionEndpoints();
app.MapListEndpoints();
app.MapUserEndpoints();
app.MapGameEndpoints();

app.UseOutputCache();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.UseStatusCodePages();
app.Run();
