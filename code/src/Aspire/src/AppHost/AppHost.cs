using System.Threading;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Yarp;
using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddDockerComposeEnvironment("docker-compose")
    .WithDashboard(db => db.WithHostPort(8085));
    /*
    .ConfigureComposeFile(file =>
    {
        file.Name = "ludus";
    });
*/
//var pgUsername = builder.AddParameter("pg-username", "postgres", secret: false);
//var pgPassword = builder.AddParameter("pg-password", "mypassword", secret: true);

//var IGDB_CLIENT_ID = builder.AddParameter("igdb-client-id", secret: true);
//var IGDB_CLIENT_SECRET = builder.AddParameter("igdb-client-secret", secret: true);

//var IGDB_ACCESSTOKEN = builder.AddParameter("igdb-token", secret: true);
//.WithUserName(postgresUsernameParameter)
//.WithPassword(postgresPasswordParameter)


var catalogPrimaryDb = builder
    .AddPostgres("catalog-primary")
    .WithDockerfile("../../../../../docker")
    //.WithImage(image: "paradedb/paradedb", tag: "v0.19.5-pg17")
    //.WithArgs("-c", "wal_level=logical")
    //.WithArgs("-c", "max_replication_slots=4")
    //.WithArgs("-c", "max_wal_senders=4")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("catalog-primary-data", isReadOnly: false)
    .AddDatabase("catalog");
/*
var catalogReplicaDb = builder
    .AddPostgres("catalog-replica")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("catalog-replica-data", isReadOnly: false)
    .AddDatabase("catalog");
*/

//.WithImage(image: "paradedb/paradedb", tag: "v0.19.3-pg17")
//.WithDockerfile("../../../docker")
/*
var webhookSecret = builder.AddParameter("webhook-secret");
var hostDomain = builder.AddParameter("host-domain");

#pragma warning disable ASPIREINTERACTION001
var adminKey = builder
    .AddParameter("adminkey", secret: true)
    .WithCustomInput(x =>
        new()
        {
            Name = "AdminKey",
            Label = "Admin Key",
            InputType = InputType.Text,
            Placeholder = "Something complicated and secure",
            Required = true,
        }
    );
#pragma warning restore ASPIREINTERACTION001
*/
/*
var catalogIngester = builder
    .AddProject<Projects.Catalog_Ingester>("catalog-ingester")
    .WithExplicitStart() // check again
    .WithEnvironment("ADMIN_KEY", adminKey)
    .WithEnvironment("IGDB_CLIENT_ID", IGDB_CLIENT_ID)
    .WithEnvironment("IGDB_CLIENT_SECRET", IGDB_CLIENT_SECRET)
    .WithEnvironment("HOST_DOMAIN", hostDomain)
    .WithEnvironment("WEBHOOK_SECRET", webhookSecret);
    .WithReference(catalogPrimaryDb, "catalog-primary") // custom name for connectionstring
*/
var catalogWorker = builder
    .AddProject<Projects.Catalog_Worker>("catalog-worker")
    // start initial process command!
    // ONE COMMAND, ONE PROCESS!
    .WithReference(catalogPrimaryDb, "catalog-primary");
    
    //.WithReference(catalogIngester);
/*
var catalogApi = builder
    .AddProject<Projects.CatalogAPI>("catalog-api")
    .WithExplicitStart()
    .WithEnvironment("ADMIN_KEY", adminKey)
    .WithHttpHealthCheck("/health");
    //.WithReference(catalogReplica)
    //.WaitFor(catalogReplica);
adminKey.WithParentRelationship(catalogApi);
*/
/*
var userDb = builder.AddPostgres("user-db")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume(isReadOnly: false); //.WithPgAdmin().WithPgWeb();
var apiServiceDatabase = mainPostgres.AddDatabase("maindb");
var apiService = builder.AddProject<Projects.Ludus.Api>("apiservice")
    .WithReference(apiServiceDatabase);

// https://aspire.dev/whats-new/aspire-13/#javascript-as-a-first-class-citizen
// https://github.com/davidfowl/aspire-ai-chat-demo/tree/main/AIChat.AppHost
// https://learn.microsoft.com/en-us/dotnet/aspire/whats-new/dotnet-aspire-9.5#yarp-static-files-support
if (builder.ExecutionContext.IsRunMode)
{
    var resource =
        builder.AddPnpmApp(
            name: "webui",
            workingDirectory: "../WebUI",
            scriptName: "dev"
        );

    resource
        .WithPnpmPackageInstallation()
        //.WithHttpsEndpoint(env: "PORT")
        .WithHttpEndpoint(env: "PORT")
        .WithHttpHealthCheck()
        .WithExternalHttpEndpoints()
        .WithOtlpExporter()
        .WithArgs(ctx =>
        {
            var targetEndpoint = resource.GetEndpoint("http");
            ctx.Args.Add("--port");
            ctx.Args.Add(targetEndpoint.Property(EndpointProperty.TargetPort));
        })
        .WithEnvironment("BROWSER", "none")
        .WithEnvironment("API_URL", apiService.GetEndpoint("https")) // for auth redirect?
        .WithEnvironment(context =>
        {
            if (context.ExecutionContext.IsRunMode)
            {
            }
        })
        .WithReference(apiService)
        .WaitFor(apiService);
}


builder.AddYarp("web-ui")
    .WithStaticFiles()
    .WithExternalHttpEndpoints()
    .WithDockerfile("../WebUI")
    .WithConfiguration(c =>
    {
        c.AddRoute("/api/{**catch-all}", apiService.GetEndpoint("http"));

        c.AddRoute("/search/{**catch-all}", syncService)
            .WithMatchMethods("GET", "POST")
            .WithTransformPathRemovePrefix("/search");
    })
    .WithExplicitStart();
*/
builder.Build().Run();
