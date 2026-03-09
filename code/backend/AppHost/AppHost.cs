using Aspire.Hosting.Yarp.Transforms;
// HTTPS
// "ASPIRE_DASHBOARD_MCP_ENDPOINT_URL": "https://localhost:23015", 
// HTTP
// "ASPIRE_DASHBOARD_MCP_ENDPOINT_URL": "http://localhost:18046",
var builder = DistributedApplication.CreateBuilder(args);
builder
    .AddDockerComposeEnvironment("env")
    .WithDashboard(db => db.WithHostPort(8085))
    .ConfigureComposeFile(file =>
    {
        file.Name = "game-index";
    });

var pgUsername = builder.AddParameter("pg-username", "postgres", secret: false);
var pgPassword = builder.AddParameter("pg-password", "PQDF13*7dpR-Q77nmQZh*3", secret: true);

var catalogDb = builder
    .AddPostgres(name: "catalog-primary", pgUsername, pgPassword)
    .WithHostPort(5433)
    .WithImage(image: "paradedb/paradedb", tag: "v0.21.10-pg17")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("catalog-primary-data", isReadOnly: false)
    .WithEndpoint(
        "tcp",
        ep =>
        {
            ep.Port = 5433;
            ep.TargetPort = 5432;
            ep.IsProxied = false;
        }
    )
    .AddDatabase("catalogdev");

var api = builder
    .AddProject<Projects.CatalogAPI>("catalogapi")
    .WaitFor(catalogDb)
    .WithReference(catalogDb);

//.WithExternalHttpEndpoints();

var frontend = builder
    .AddViteApp("frontend", "../../web/apps/game-index", runScriptName: "dev2")
    .WithPnpm()
    .WithReference(api)
    .WaitFor(api);
    
    //.WithUrl("", "Game Index");

/*
.WithEndpoint(
    endpointName: "http",
    endpoint =>
    {
        endpoint.Port = builder.ExecutionContext.IsRunMode ? 5173 : null;
    }
);
*/

//if (builder.ExecutionContext.IsPublishMode)
//{
builder
    .AddYarp("frontend-server")
    .WithExternalHttpEndpoints()
    .PublishWithStaticFiles(frontend)
    .WithConfiguration(yarp =>
    {
        // Always proxy /api requests to backend
        yarp.AddRoute("/api/{**catch-all}", api); //.WithTransformPathRemovePrefix("/api");
    });
    
    //.WithExplicitStart();

//.WithExternalHttpEndpoints()

//}
builder.Build().Run();
