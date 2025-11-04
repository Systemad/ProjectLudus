using Aspire.Hosting.Yarp;
using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddDockerComposeEnvironment("env")
    .WithDashboard(db => db.WithHostPort(8085))
    .ConfigureComposeFile(file =>
    {
        file.Name = "ludus";
    });

var IGDB_CLIENT_ID = builder.AddParameter("igdb-client-id", secret: true);
var IGDB_CLIENT_SECRET = builder.AddParameter("igdb-client-secret", secret: true);

//var IGDB_ACCESSTOKEN = builder.AddParameter("igdb-token", secret: true);

// isolate sync service completely!
//  postgresql-17-pg-search_0.18.2-1PARADEDB-noble_amd64.deb
var catalogDb = builder
    .AddPostgres("catalog-postgres")
    //.WithImage(image: "paradedb/paradedb", tag: "v0.19.3-pg17")
    .WithDockerfile("../../../docker")
    .WithDataVolume("paradedb-testing",  isReadOnly: false) /*, "/var/lib/postgresql")*/
    .AddDatabase("catalog-db");

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

var catalogApi = builder
    .AddProject<Projects.CatalogAPI>("catalog-api")
    .WithExplicitStart()
    .WithEnvironment("ADMIN_KEY", adminKey)
    .WithEnvironment("IGDB_CLIENT_ID", IGDB_CLIENT_ID)
    .WithEnvironment("IGDB_CLIENT_SECRET", IGDB_CLIENT_SECRET)
    .WithHttpCommand(
        path: "admin/queue/start",
        displayName: "Start queue",
        commandOptions: new HttpCommandOptions()
        {
            IconName = "DatabaseFilled",
            IconVariant = IconVariant.Filled,
            ConfirmationMessage = "Do you want to start queue to process webhook messages?",
            PrepareRequest = async (context) =>
            {
                context.Request.Headers.Add(
                    "X-AdminKey",
                    $"{await adminKey.Resource.GetValueAsync(CancellationToken.None)}"
                );
            },
            IsHighlighted = false,
        }
    )
    .WithHttpCommand(
        path: "admin/queue/stop",
        displayName: "Start queue",
        commandOptions: new HttpCommandOptions()
        {
            IconName = "DatabaseFilled",
            IconVariant = IconVariant.Filled,
            ConfirmationMessage = "Do you want to start queue to process webhook messages?",
            PrepareRequest = async (context) =>
            {
                context.Request.Headers.Add(
                    "X-AdminKey",
                    $"{await adminKey.Resource.GetValueAsync(CancellationToken.None)}"
                );
            },
            IsHighlighted = false,
        }
    )
    .WithHttpHealthCheck("/health")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

adminKey.WithParentRelationship(catalogApi);

/*
var mainPostgres = builder.AddPostgres("mainPostgres").WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume(isReadOnly: false); //.WithPgAdmin().WithPgWeb();
var apiServiceDatabase = mainPostgres.AddDatabase("maindb");
var apiService = builder.AddProject<Projects.WebAPI>("apiservice")
    .WithReference(apiServiceDatabase);


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
