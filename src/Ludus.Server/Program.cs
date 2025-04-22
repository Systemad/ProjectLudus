using Ludus.Server.Features;
using Ludus.Server.Features.Auth;
using Ludus.Server.Features.Games;
using Ludus.Server.Features.User;
using Ludus.Server.Features.User.Status;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using SteamWebAPI2.Utilities;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddMarten(options =>
    {
        options.Connection(
            "host=localhost:5432;database=gamingdb;password=Compaq2009;username=dan1"
        );
        options.UseSystemTextJsonForSerialization();

        options.OpenTelemetry.TrackConnections = TrackLevel.Normal;

        options.Schema.For<Game>().FullTextIndex(x => x.Name);
        options.Schema.For<Game>().Identity(x => x.Id);

        options.Schema.For<User>().Identity(x => x.Id);
        options.Schema.For<UserGameStatus>().Identity(x => x.Id);
        options.Schema.For<UserImage>().Identity(x => x.Id);

        if (builder.Environment.IsDevelopment())
        {
            options.AutoCreateSchemaObjects = AutoCreate.All;
        }
    })
    .ApplyAllDatabaseChangesOnStartup();

//.OptimizeArtifactWorkflow()
//.InitializeWith();

builder.Services.AddMartenStore<IUserStore>(options =>
{
    options.Connection("host=localhost:5432;database=userdb;password=Compaq2009;username=dan1");
    options.UseSystemTextJsonForSerialization();
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});

//.OptimizeArtifactWorkflow()
//.InitializeWith();

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

ConfigureOpenTelemetry(builder);

static IHostApplicationBuilder ConfigureOpenTelemetry(IHostApplicationBuilder builder)
{
    builder
        .Services.AddOpenTelemetry()
        .WithTracing(tracing =>
        {
            tracing.AddHttpClientInstrumentation();
            tracing.AddSource("Marten");
        })
        .WithMetrics(metrics =>
        {
            metrics.AddHttpClientInstrumentation().AddRuntimeInstrumentation();
            metrics.AddMeter("Marten");
        });
    var useOtlpExporter = !string.IsNullOrWhiteSpace(
        builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]
    );
    if (useOtlpExporter)
    {
        builder.Services.AddOpenTelemetry().UseOtlpExporter();
    }

    return builder;
}

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

//app.Map("/", () => Results.Redirect("/scalar/v1"));

app.MapAuthEndpoints();

app.MapGameEndpoints();

app.MapUserEndpoints();

app.MapRazorPages();
app.UseOutputCache();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
