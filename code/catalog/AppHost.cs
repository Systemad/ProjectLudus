#!/usr/bin/env dotnet run

#:sdk Aspire.AppHost.Sdk@13.4.6
#:package Aspire.Hosting.PostgreSQL@13.4.6
#:package Aspire.Hosting.Docker@13.4.6
#:property ExperimentalFileBasedProgramEnableIncludeDirective=true
#:property ExperimentalFileBasedProgramEnableTransitiveDirectives=true

#pragma warning disable ASPIRECSHARPAPPS001

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Docker.Resources.ComposeNodes;
using Aspire.Hosting.Pipelines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

var builder = DistributedApplication.CreateBuilder(args);

var compose = builder.AddDockerComposeEnvironment("compose");

compose.ConfigureComposeFile(f =>
{
    f.Networks.Add("dokploy-network", new Network { Name = "dokploy-network", External = true });
});

var pgHost = builder.AddParameter("pg-host");
var pgPort = builder.AddParameter("pg-port");
var pgUser = builder.AddParameter("pg-user");
var pgPass = builder.AddParameter("pg-password", secret: true);
var pgDb = builder.AddParameter("pg-database");

var prefectApiUrl = builder.AddParameter("prefect-api-url");
var prefectUiUrl = builder.AddParameter("prefect-ui-url");

var igdbClientId = builder.AddParameter("igdb-client-id");
var igdbAccessToken = builder.AddParameter("igdb-access-token", secret: true);
var steamApiKey = builder.AddParameter("steam-api-key", secret: true);
var webhookSecret = builder.AddParameter("webhook-secret", secret: true);
var typesenseApiKey = builder.AddParameter("typesense-api-key", secret: true);
var umamiApiKey = builder.AddParameter("umami-api-key", secret: true);
var webhookUrl = builder.AddParameter("webhook-url");

var prefectUrl = ReferenceExpression.Create(
    $"postgresql+asyncpg://{pgUser.Resource}:{pgPass.Resource}@{pgHost.Resource}:{pgPort.Resource}/prefect"
);

var migrationConnectionString = ReferenceExpression.Create(
    $"Host={pgHost.Resource};Port={pgPort.Resource};Username={pgUser.Resource};Password={pgPass.Resource};Database={pgDb.Resource}"
);

/*
var webhook = builder
    .AddCSharpApp("igdb-webhook", "./webhook/App.cs")
    .WithEnvironment("IGDB__CLIENT_ID", igdbClientId)
    .WithEnvironment("IGDB__ACCESS_TOKEN", igdbAccessToken)
    .WithEnvironment("IGDB__WEBHOOK_SECRET", webhookSecret)
    .WithEnvironment("IGDB__WEBHOOK_URL", webhookUrl)
    .WithEndpoint(port: 8000);
*/
var grateMigration = builder
    .AddDotnetTool("grate-migration", "grate")
    .WithArgs(
        "--databasetype",
        "postgresql",
        "--connectionstring",
        migrationConnectionString,
        "--sqlfilesdirectory",
        "./infrastructure/db/grate",
        "--silent"
    );

if (builder.ExecutionContext.IsRunMode)
{
    var pg = builder
        .AddPostgres("pg", userName: pgUser, password: pgPass, port: 5433)
        .WithImage("timescale/timescaledb")
        .WithImageTag("2.27.2-pg17")
        .WithEnvironment("POSTGRES_DB", pgDb)
        .WithDataVolume("pg-test-data");
    //.WithLifetime(ContainerLifetime.Persistent);

    var catalogdb = pg.AddDatabase("catalogdb");
    var prefectdb = pg.AddDatabase("prefect");

    //webhook.WithReference(catalogdb).WaitFor(pg);

    // THIS is the important dependency
    grateMigration.WaitFor(pg);

    prefectUrl = ReferenceExpression.Create(
        $"postgresql+asyncpg://{pgUser}:{pgPass}@{pg.Resource.Host}:{pg.Resource.Port}/prefect"
    );

    pg.OnResourceReady(
        (resource, @event, cancellationToken) =>
        {
            //prefectUrl = ReferenceExpression.Create(
            //    $"postgresql+asyncpg://{pgUser}:{pgPass}@{pg.Resource.Host}:{pg.Resource.Port}/prefect"
            //);
            return Task.CompletedTask;
        }
    );
}

#pragma warning disable ASPIREPIPELINES001
builder.Pipeline.AddStep(
    "migrate-database",
    async (context) =>
    {
        context.Logger.LogInformation("Migrations completed.");
    },
    requiredBy: WellKnownPipelineSteps.Deploy
);
#pragma warning restore ASPIREPIPELINES001

//webhook.WaitFor(grateMigration);
#pragma warning disable ASPIREPIPELINES003
var prefectServer = builder
    .AddContainer("prefect-server", "prefecthq/prefect:3-python3.13")
    .WithArgs("prefect", "server", "start", "--host", "0.0.0.0", "--port", "4200")
    .WithHttpEndpoint(targetPort: 4200, port: 4200, name: "http")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/api/health")
    .WithEnvironment("PREFECT_API_DATABASE_CONNECTION_URL", prefectUrl)
    .WithEnvironment("PREFECT_API_URL", prefectApiUrl)
    .WithEnvironment("PREFECT_UI_URL", prefectUiUrl)
    .WaitFor(grateMigration)
    .PublishAsDockerComposeService(
        (resource, service) =>
        {
            service.Networks = new List<string> { "aspire", "dokploy-network" };
        }
    );

var pipeline = builder
    .AddContainer("pipeline-worker", "prefecthq/prefect", "3-python3.13")
    .WithArgs("prefect", "worker", "start", "--pool", "pipeline-pool", "--type", "docker")
    .WithBindMount("/var/run/docker.sock", "/var/run/docker.sock")
    .WithVolume("dlt-data", "/data/dlt")
    .WithEnvironment("PREFECT_API_URL", prefectApiUrl)
    .WithEnvironment("PREFECT_UI_URL", prefectUiUrl)
    /*
    .WithEnvironment("PREFECT_API_URL", "http://prefectServer:4200/api")
    .WithEnvironment("IGDB__CLIENT_ID", igdbClientId)
    .WithEnvironment("IGDB__ACCESS_TOKEN", igdbAccessToken)
    .WithEnvironment("STEAM__API_KEY", steamApiKey)
    .WithEnvironment("DESTINATION__TYPESENSE__CREDENTIALS__API_KEY", typesenseApiKey)
    .WithEnvironment("UMAMI__API_KEY", umamiApiKey)
    .WithEnvironment("DLT_DATA_DIR", "/data/dlt")
    .WithEnvironment("DATA_WRITER__BUFFER_MAX_ITEMS", "500")
    .WithEnvironment("DATA_WRITER__FILE_MAX_ITEMS", "5000")
    .WithEnvironment("DATA_WRITER__FILE_MAX_BYTES", "500000")
    .WithEnvironment("NORMALIZE__WORKERS", "1")
    .WithEnvironment("DESTINATION__POSTGRES__CREDENTIALS__HOST", pgHost)
    .WithEnvironment("DESTINATION__POSTGRES__CREDENTIALS__PORT", pgPort)
    .WithEnvironment("DESTINATION__POSTGRES__CREDENTIALS__USERNAME", pgUser)
    .WithEnvironment("DESTINATION__POSTGRES__CREDENTIALS__PASSWORD", pgPass)
    .WithEnvironment("DESTINATION__POSTGRES__CREDENTIALS__DATABASE", pgDb)
    */
    .WaitFor(prefectServer)
    .WaitFor(grateMigration)
    .PublishAsDockerComposeService(
        (resource, service) =>
        {
            service.Networks = new List<string> { "aspire", "dokploy-network" };
        }
    );

#pragma warning restore ASPIREPIPELINES003

builder.Build().Run();
