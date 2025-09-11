var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddDockerComposeEnvironment("env")
    .ConfigureComposeFile(file => { file.Name = "ludus-web"; });

// isolate sync service completely!
var syncPostgres = builder.AddPostgres("syncPostgres").WithDataVolume(); //.WithPgAdmin().WithPgWeb();

var syncWorkerDatabase = syncPostgres.AddDatabase("syncdb");
var syncWorkerMigrations = builder
    .AddProject<Projects.SyncService_MigrationService>("syncMigrations")
    .WithReference(syncWorkerDatabase).WaitFor(syncWorkerDatabase);
var syncService = builder
    .AddProject<Projects.SyncService>("syncService")
    .WithReference(syncWorkerMigrations)
    .WaitForCompletion(syncWorkerMigrations);


// TODO: CREATE MIGRATION PROJECT AND DATA HERE AS WELL? OR JUST ASS SERVICE INTO PROJECT
var mainPostgres = builder.AddPostgres("mainPostgres").WithDataVolume(); //.WithPgAdmin().WithPgWeb();
var apiServiceDatabase = mainPostgres.AddDatabase("maindb");
var apiService = builder.AddProject<Projects.WebAPI>("apiservice")
    .WithReference(apiServiceDatabase);

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

builder.Build().Run();