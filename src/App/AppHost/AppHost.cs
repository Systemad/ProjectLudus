var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddDockerComposeEnvironment("env")
    .ConfigureComposeFile(file =>
    {
        file.Name = "ludus-web";
    });

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
    //.WithHttpEndpoint(env: "VITE_PORT")
    //.WithExternalHttpEndpoints()
    //.WithHttpEndpoint(env: "PORT")
    //.WithReverseProxy(webApi.GetEndpoint("http"))
    //.WithExternalHttpEndpoints()
    .WithOtlpExporter()
    .WithEnvironment("BROWSER", "none")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
