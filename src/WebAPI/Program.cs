using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SteamWebAPI2.Utilities;
using WebAPI.Configuration;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Games.Services;
using WebAPI.Features.Common.Lists.Services;
using WebAPI.Features.DataAccess;

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
    opt.UseNpgsql("host=localhost:5432;database=ludusdb;password=Compaq2009;username=dan1")
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

builder.Services.AddMartenDatabases(builder.Environment, builder.Configuration);

builder.Services.AddHttpClient();

/*
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(5);
});
*/
//builder.Services.AddExceptionHandler<GlobalExceptionsHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddTransient(x => new SteamWebInterfaceFactory(
    builder.Configuration["SteamWebAPIKey"]
));

builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IUserListService, UserListService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "/openapi/v1.json");
    app.MapScalarApiReference();

    //app.MapOpenApi();
    //app.Map("/", () => Results.Redirect("/scalar/v1"));
}

//app.UseDefaultFiles();
//app.UseStaticFiles();
//app.MapStaticAssets();

app.UseDefaultExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.UseFastEndpoints(x =>
    {
        x.Errors.UseProblemDetails();
        //x.Endpoints.ShortNames = true;
    })
    .UseSwaggerGen(c => { });

app.UseOutputCache();

//app.MapControllers();
app.UseStatusCodePages();

//app.MapFallbackToFile("/index.html");

app.Run();
