using Aspire.Hosting.Yarp;
using Aspire.Hosting.Yarp.Transforms;

var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddDockerComposeEnvironment("env")
    .WithDashboard(db => db.WithHostPort(8085))
    .ConfigureComposeFile(file => { file.Name = "ludus-web"; });

var IGDB_CLIENT = builder.AddParameter("igdb-client", secret: true);
var IGDB_ACCESSTOKEN = builder.AddParameter("igdb-token", secret: true);


// isolate sync service completely!
//  postgresql-17-pg-search_0.18.2-1PARADEDB-noble_amd64.deb 
var gamingdb =
    builder.AddPostgres("syncPostgres")
        .WithImage(image: "paradedb/paradedb", tag: "v0.18.11-pg17")
        //.WithAnnotation(new ContainerImageAnnotation() { Image = "paradedb/paradedb", Tag = "v0.18.2-pg17" })
        .WithLifetime(ContainerLifetime.Persistent)
        //.WithImageTag("17")
        .WithVolume("paradedb", "/var/lib/postgresql")
        //.WithDataVolume(isReadOnly: false)
        .AddDatabase("syncdb");

var syncService = builder
    .AddProject<Projects.SyncService>("syncService")
    .WithEnvironment("IGDB_CLIENT", IGDB_CLIENT)
    .WithEnvironment("IGDB_ACCESSTOKEN", IGDB_ACCESSTOKEN)
    .WithReference(gamingdb)
    .WaitFor(gamingdb);

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