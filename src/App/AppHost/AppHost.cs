var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddDockerComposeEnvironment("env")
    .ConfigureComposeFile(file => { file.Name = "ludus-web"; });
/*
 * host=localhost:5432;database=ludusdb;password=Compaq2009;username=dan1
 *
 */
var postgres = builder.AddPostgres("postgres"); //.WithInitFiles("./postgres-init");
var postgresdb = postgres.AddDatabase("maindb");

//var apiUrl = builder.AddParameter("postgresl");
//var externalDb = builder.AddExternalService("external-db", apiUrl);

var connectionString = builder.AddConnectionString("gamingdb");

var apiService = builder.AddProject<Projects.WebAPI>("apiservice").WithReference(postgresdb)
    .WithEnvironment("IGDB_DB", connectionString); //.WithReference(connectionString);
var webui = builder
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

//var webEndppint = webui.Resource.GetEndpoint("http");
//apiService.WithEnvironment("WEBUI__ENDPOINT", webEndppint);

apiService.WithReference(webui);

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