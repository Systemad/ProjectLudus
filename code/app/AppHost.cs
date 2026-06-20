#:sdk Aspire.AppHost.Sdk@13.3.5
#:package Aspire.Hosting.Yarp@13.3.5
#:package Aspire.Hosting.JavaScript@13.3.5
#:package Aspire.Hosting.Docker@13.3.5

// #:property TargetFramework=net11.0

// #:package Aspire.Hosting.Python@13.3.3

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
// the AddConnectionString fallback on catalogApi.
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

// Frontend reads Typesense config from VITE_TYPESENSE_* env vars at build time.
// See apps/game-index/src/features/search/instantsearch.ts for fallback defaults.
// For local dev, set in apps/game-index/.env:
//   VITE_TYPESENSE_HOST=localhost
//   VITE_TYPESENSE_API_KEY=your-key

var catalogApi = builder
    .AddProject("catalogApi", "./backend/CatalogAPI")
    .WithReference(catalogConnectionString)
    .WithHttpsEndpoint();

#pragma warning disable ASPIREJAVASCRIPT001
var frontend = builder
    .AddJavaScriptApp("frontend", "./frontend", runScriptName: "dev")
    .WithPnpm()
    .WithReference(catalogApi)
    .WaitFor(catalogApi)
    .PublishAsStaticWebsite(
        "/catalog",
        catalogApi,
        opts => opts.OutputPath = "apps/game-index/dist"
    );
#pragma warning restore ASPIREJAVASCRIPT001

builder.Build().Run();
