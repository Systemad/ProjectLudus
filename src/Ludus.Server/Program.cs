using FastEndpoints;
using Ludus.Server.Configuration;
using Ludus.Server.Features.Lists.Services;
using Scalar.AspNetCore;
using SteamWebAPI2.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuth();

builder.Services.AddFastEndpoints();
builder.Services.AddMartenDatabases(builder.Environment, builder.Configuration);

builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(5);
});

//builder.Services.AddExceptionHandler<GlobalExceptionsHandler>();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();
builder.Services.AddControllers();

/*
 builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});
*/
builder.Services.AddTransient(x => new SteamWebInterfaceFactory(
    builder.Configuration["SteamWebAPIKey"]
));

//builder.Services.ConfigureOpenTelemetry(builder.Configuration);

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
    //app.Map("/", () => Results.Redirect("/scalar/v1"));
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapStaticAssets();

app.UseDefaultExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();

//app.MapStaticAssets();
//app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.UseFastEndpoints(x =>
{
    x.Errors.UseProblemDetails();
});

//app.MapRazorPages();

app.UseOutputCache();

app.MapControllers();
app.UseStatusCodePages();

app.MapFallbackToFile("/index.html");

app.Run();
