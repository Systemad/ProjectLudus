var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddDockerComposeEnvironment("env")
    .ConfigureComposeFile(file => { file.Name = "ludus-web"; });

/*
 * host=localhost:5432;database=ludusdb;password=Compaq2009;username=dan1
 *
 */
var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("maindb");

var apiService = builder.AddProject<Projects.WebAPI>("apiservice").WithReference(postgresdb);
builder
    .AddViteApp(
        name: "webui",
        workingDirectory: "../WebUI",
        packageManager: "pnpm",
        useHttps: false
    )
    .WithPnpmPackageInstallation()
    .WithHttpHealthCheck()
    .WithExternalHttpEndpoints()
    .WithOtlpExporter()
    .WithEnvironment("BROWSER", "none")
    .WithEnvironment(context =>
    {
        if (context.ExecutionContext.IsRunMode)
        {

        }
    })
    .WithReference(apiService)
    .WaitFor(apiService);

/*
builder.AddPnpmApp("webui", "../WebUI", "dev")
    .WithPnpmPackageInstallation()
    .WithHttpEndpoint(5173)
    .WithEnvironment("BROWSER", "none")
    .WithOtlpExporter()
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);
*/
builder.Build().Run();
//.WithReverseProxy(webApi.GetEndpoint("http"))
//.WithHttpEndpoint(env: "VITE_PORT")
//.WithHttpEndpoint(port: 54570)
//.WithHttpEndpoint(54570)
//.WithExternalHttpEndpoints()