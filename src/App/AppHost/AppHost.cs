var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddDockerComposeEnvironment("env")
    .ConfigureComposeFile(file => { file.Name = "ludus-web"; });
/*
 * host=localhost:5432;database=ludusdb;password=Compaq2009;username=dan1
 */
var postgres = builder.AddPostgres("postgres").WithDataVolume().WithPgAdmin().WithPgWeb();

var postgresdb = postgres.AddDatabase("maindb");

// SHOULD NOT ADD DB DIRECTLY, REFERENCE WORKER PROJECT
//var connectionString = builder.AddConnectionString("gamingdb");


var apiService = builder.AddProject<Projects.WebAPI>("apiservice")
    .WithReference(postgresdb);
    
    //.WithEnvironment("IGDB_DB", connectionString);

//var apiUrl = builder.AddParameter("postgresl");
//var externalDb = builder.AddExternalService("external-db", apiUrl);

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