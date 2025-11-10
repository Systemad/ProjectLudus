using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SteamWebAPI2.Utilities;
using Ludus.Api.Features.Auth.Extensions;
using Ludus.Api.Features.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Logging.AddConsole();

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
        options.DocumentSettings = s => { s.MarkNonNullablePropsAsRequired(); };
    });

builder.AddNpgsqlDbContext<LudusContext>(connectionName: "maindb", configureDbContextOptions: (optionsBuilder) =>
{
    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services.AddTransient(x => new SteamWebInterfaceFactory(
    builder.Configuration["SteamWebAPIKey"]
));

builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseFastEndpoints(x =>
    {
        x.Errors.UseProblemDetails();
        //x.Endpoints.ShortNames = true;
    })
    .UseSwaggerGen(c => { });

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "/openapi/v1.json");
    app.MapScalarApiReference();

    //app.MapOpenApi();
    app.Map("/scalar", () => Results.Redirect("/scalar/v1"));
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


//app.UseOutputCache();

//app.MapControllers();
app.UseStatusCodePages();

//app.MapFallbackToFile("/index.html");

app.Run();