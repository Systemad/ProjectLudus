using FastEndpoints;
using FastEndpoints.Swagger;
using Ludus.Server.Configuration;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Games.Services;
using Ludus.Server.Features.Common.Lists.Services;
using Ludus.Server.Features.DataAccess;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SteamWebAPI2.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationCookie(validFor: TimeSpan.FromDays(7)).AddAuthorization();

builder
    .Services.AddFastEndpoints()
    .SwaggerDocument(options =>
    {
        options.DocumentSettings = s =>
        {
            s.Title = "Ludus API";
            s.Version = "v1";
        };
        options.ShortSchemaNames = true;
        options.DocumentSettings = s =>
        {
            s.MarkNonNullablePropsAsRequired();
        };
    });

builder.Services.AddDbContextPool<LudusContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "host=localhost:5432;database=ludusdb;password=Compaq2009;username=dan1"
        )
    )
);

builder.Services.AddMartenDatabases(builder.Environment, builder.Configuration);

//builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

/*
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(5);
});
*/
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
builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IUserListService, UserListService>();
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
    app.UseOpenApi(c => c.Path = "/openapi/v1.json");
    app.MapScalarApiReference();

    //app.MapOpenApi();
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
        //x.Endpoints.ShortNames = true;
    })
    .UseSwaggerGen(c => { });

//app.MapRazorPages();

app.UseOutputCache();

app.MapControllers();
app.UseStatusCodePages();

app.MapFallbackToFile("/index.html");

app.Run();
