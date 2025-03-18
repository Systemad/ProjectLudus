using Ludus.Data;
using Ludus.Server;
using Ludus.Server.Features.Auth;
using Ludus.Server.Features.Games;
using Ludus.Server.Features.User;
using Marten;
using Microsoft.AspNetCore.Authentication.Cookies;
using Scalar.AspNetCore;
using SteamWebAPI2.Utilities;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(options =>
{
    options.Connection("host=localhost:5432;database=gamingdb;password=Compaq2009;username=dan1");
    options.UseSystemTextJsonForSerialization();
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});

builder.Services.AddDbContext<AppDbContext>();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/signin";
        options.LogoutPath = "/signout";
        options.AccessDeniedPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.Events.OnSignedIn = ValidationHelper.SignIn;
        options.Events.OnValidatePrincipal = ValidationHelper.Validate;
    })
    .AddSteam();

builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddOutputCache();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddTransient(x => new SteamWebInterfaceFactory(
    builder.Configuration["SteamWebAPIKey"]
));

var app = builder.Build();

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
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapStaticAssets();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();

app.Map("/", () => Results.Redirect("/scalar/v1"));

app.MapAuthEndpoints();

app.MapGameEndpoints();

app.MapUserEndpoints();

app.MapRazorPages();
app.UseOutputCache();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
