#:sdk Aspire.AppHost.Sdk@13.4.6
#:package Aspire.Hosting.Yarp@13.4.6
#:package Aspire.Hosting.JavaScript@13.4.6
#:package Aspire.Hosting.Docker@13.4.6
#:package Aspire.Hosting.Browsers@13.4.6-preview.1.26319.6
#:package Aspire.Hosting.DevTunnels@13.4.6
#:package Aspire.Hosting.EntityFrameworkCore@13.4.6-preview.1.26319.6
#:property TargetFramework=net11.0
#:property AspireUseCliBundle=true

// #:package Aspire.Hosting.Python@13.3.3

using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env").WithDashboard(db => db.WithHostPort(8085));

// --- Typesense runs externally on VPS (see catalog/docker-compose.yml or Dokploy service) ---
// To run locally via Aspire, uncomment the block below and set VITE_TYPESENSE_HOST accordingly.
//
// var typesenseMasterKey = builder.AddParameter("TYPESENSE-ADMIN-KEY");
// var typesenseDataPath = Path.Combine(
//     Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
//     "typesense-data"
// );
// builder
//     .AddContainer("typesense", "typesense/typesense:30.2")
//     .WithEndpoint("http", c => { c.Port = 8108; c.TargetPort = 8108; })
//     .WithBindMount(typesenseDataPath, "/data")
//     .WithArgs("--data-dir", "/data", "--enable-cors", "--enable-search-analytics",
//         "--analytics-dir", "/data/analytics", "--analytics-minute-rate-limit", "500")
//     .WithEnvironment("TYPESENSE_API_KEY", typesenseMasterKey)
//     .PublishAsDockerComposeService((resource, service) => { service.Name = "typesense"; service.Restart = "on-failure"; })
//     .WithLifetime(ContainerLifetime.Persistent);

// --- Catalog data infrastructure (Postgres + Prefect) runs externally ---
// See docker-compose.yml in code/catalog/ for the standalone deployment.
// To run Postgres locally via Aspire, uncomment the block below and remove
// the AddConnectionString fallback on backendApi.
//
// var pgUsername = builder.AddParameter("pg-username", "postgres", secret: false);
// var pgPassword = builder.AddParameter("pg-password", secret: true);
// var postgres = builder
//     .AddPostgres("postgres", pgUsername, pgPassword)
//     .WithImage("timescale/timescaledb", tag: "2.27.0-pg17")
//     .WithHostPort(5433)
//     .WithLifetime(ContainerLifetime.Persistent)
//     .WithDataVolume();
// var catalogDb = postgres.AddDatabase("catalogdb");
// var prefectDb = postgres.AddDatabase("prefect");
// var migration = builder
//     .AddExecutable("db-migration", "dotnet", "grate",
//         "--connection-string", catalogDb.Resource.ConnectionStringExpression,
//         "--database-type", "postgresql",
//         "--folder", "infrastructure/db/grate")
//     .WithEnvironment("DOTNET_ENVIRONMENT", "migration")
//     .WaitFor(catalogDb);

var catalogConnectionString = builder.AddConnectionString("catalogdb");
var playConnectionString = builder.AddConnectionString("playdb");
var notificationConnectionString = builder.AddConnectionString("notificationsdb");
var steamApiKey = builder.AddParameter("steam-api-key", secret: true);
var typesense = builder
    .AddExternalService("typesense", "http://neptune.tail78fa0d.ts.net:8108")
    .WithHttpHealthCheck("/health");
var typesenseProxy = builder
    .AddYarp("typesense-proxy")
    .WithHostPort(8110)
    .WithConfiguration(proxy => proxy.AddRoute(typesense));

// Frontend reads Typesense config from VITE_TYPESENSE_* env vars at build time.
// See apps/game-index/src/features/search/instantsearch.ts for fallback defaults.
// For local dev, set in apps/game-index/.env:
//   VITE_TYPESENSE_HOST=localhost
//   VITE_TYPESENSE_API_KEY=your-key

var backendApi = builder
    .AddProject("backendApi", "./backend/Backend.API")
    .WithReference(catalogConnectionString)
    .WithReference(playConnectionString)
    .WithEnvironment("Steam__ApiKey", steamApiKey)
    .WithHttpsEndpoint();

var playMigrations = backendApi
    .AddEFMigrations("play-migrations", "Play.Infrastructure.Persistence.PlayDbContext")
    .WithReference(playConnectionString)
    .RunDatabaseUpdateOnStart()
    .PublishAsMigrationBundle(publishContainer: true);

backendApi.WaitForCompletion(playMigrations);

var notifications = builder
    .AddProject("notifications", "./backend/Notifications")
    .WithReference(catalogConnectionString)
    .WithReference(notificationConnectionString);

var notificationMigrations = notifications
    .AddEFMigrations(
        "notification-migrations",
        "Notifications.Infrastructure.Persistence.NotificationDbContext"
    )
    .WithReference(notificationConnectionString)
    .RunDatabaseUpdateOnStart()
    .PublishAsMigrationBundle(publishContainer: true);

notifications.WaitForCompletion(notificationMigrations);

builder
    .AddDevTunnel("mobile-api")
    .WithReference(backendApi.GetEndpoint("http"), allowAnonymous: true);

builder
    .AddDevTunnel("mobile-typesense")
    .WithReference(typesenseProxy.GetEndpoint("http"), allowAnonymous: true);

#pragma warning disable ASPIREBROWSERLOGS001, ASPIREJAVASCRIPT001
var frontend = builder
    .AddJavaScriptApp("frontend", "./frontend", runScriptName: "dev")
    .WithPnpm()
    .WithReference(backendApi)
    .WaitFor(backendApi)
    .WithHttpEndpoint(name: "http", targetPort: 5173)
    .WithBrowserLogs(browser: "chrome")
    .PublishAsStaticWebsite(
        "/catalog",
        backendApi,
        opts => opts.OutputPath = "apps/game-index/dist"
    )
    .WithExplicitStart;
#pragma warning restore ASPIREBROWSERLOGS001, ASPIREJAVASCRIPT001
builder.Build().Run();
