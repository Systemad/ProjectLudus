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

/*
 *
 *     public static IResourceBuilder<NodeAppResource> AddViteApp(this IDistributedApplicationBuilder builder, [ResourceName] string name, string? workingDirectory = null, string packageManager = "npm", bool useHttps = false)
    {

        return resource.WithArgs(ctx =>
        {
            if (packageManager == "npm")
            {
                ctx.Args.Add("--");
            }

            var targetEndpoint = resource.Resource.GetEndpoint(useHttps ? "https" : "http");
            ctx.Args.Add("--port");
            ctx.Args.Add(targetEndpoint.Property(EndpointProperty.TargetPort));
        });
    }
 */
/*
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
*/

//var webEndppint = webui.Resource.GetEndpoint("http");
//apiService.WithEnvironment("WEBUI__ENDPOINT", webEndppint);

//apiService.WithReference(webui);

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