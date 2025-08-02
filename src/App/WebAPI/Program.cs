using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SteamWebAPI2.Utilities;
using WebAPI.Configuration;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthenticationCookie(validFor: TimeSpan.FromDays(7)).AddAuthorization();

builder
    .Services.AddFastEndpoints()
    //.AddIdempotency()
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


builder.Services.AddDbContext<LudusContext>(options =>
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("pgdb")
            ?? throw new InvalidOperationException("Connection string 'pgdb' not found.") //, optionsBuilder => optionsBuilder.UseNodaTime() 
        );
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
);

builder.EnrichNpgsqlDbContext<LudusContext>(configureSettings: settings =>
{
    settings.DisableRetry = false;
    settings.CommandTimeout = 30;
});

builder.Services.AddMartenDatabases(builder.Environment, builder.Configuration);

builder.Services.AddHttpClient();

/*
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(5);
});
*/
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddTransient(x => new SteamWebInterfaceFactory(
    builder.Configuration["SteamWebAPIKey"]
));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "/openapi/v1.json");
    app.MapScalarApiReference();

    //app.MapOpenApi();
    app.Map("/scalar", () => Results.Redirect("/scalar/v1"));
    app.Map("/redirectweb", () => Results.Redirect("http://webui/"));
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

//app.UseOutputCache();

//app.MapControllers();
app.UseStatusCodePages();

//app.MapFallbackToFile("/index.html");
/*
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<LudusContext>();
await dbContext.Database.MigrateAsync();
*/
app.Run();