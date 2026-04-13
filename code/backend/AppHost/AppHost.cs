using System.IO;
using Aspire.Hosting.Docker.Resources.ServiceNodes.Swarm;
using Aspire.Hosting.Yarp.Transforms;
using Microsoft.Extensions.Hosting;

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

var typesenseMasterKey = builder.AddParameter("TYPESENSE-ADMIN-KEY");

var typesenseDataPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "typesense-data"
);

//var pgUsername = builder.AddParameter("pg-username", "postgres", secret: false);
//var pgPassword = builder.AddParameter("pg-password", "PQDF13*7dpR-Q77nmQZh*3", secret: true);

var postgres = builder
    .AddPostgres(
        name: "main-postgres" /*, pgUsername, pgPassword*/
    )
    .WithHostPort(5433)
    .WithImage(image: "library/postgres", tag: "18")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("game-index", isReadOnly: false)
    .WithEndpoint(
        "tcp",
        ep =>
        {
            ep.Port = 5433;
            ep.TargetPort = 5432;
            ep.IsProxied = false;
        }
    );

var catalogDb = postgres.AddDatabase("catalogdev");
var playDb = postgres.AddDatabase("playdev");

var typesense = builder
    .AddContainer("typesense", "typesense/typesense:30.2.rc9")
    .WithEndpoint(
        "http",
        c =>
        {
            c.Port = 8108;
            c.TargetPort = 8108;
        }
    )
    .WithBindMount(typesenseDataPath, "/data")
    .WithArgs(
        "--data-dir",
        "/data",
        "--enable-cors",
        "--enable-search-analytics",
        "--analytics-dir",
        "/data/analytics",
        "--analytics-minute-rate-limit",
        "500"
    )
    .WithEnvironment("TYPESENSE_API_KEY", typesenseMasterKey)
    .PublishAsDockerComposeService(
        (resource, service) =>
        {
            service.Name = "typesense";
            service.Restart = "on-failure";
        }
    );

/*
var bootstrap = builder.AddPythonApp(
        "typesense-bootstrap",
        "../python-bootstrap",
        scriptPath: "bootstrap_typesense.py"
    );
*/
//.WithReference(typesense)

var playApi = builder.AddProject<Projects.PlayAPI>("playapi").WaitFor(playDb).WithReference(playDb);
var catalogApi = builder
    .AddProject<Projects.CatalogAPI>("catalogApi")
    .WaitFor(catalogDb)
    .WithReference(catalogDb);

//.WithReference(typesense)
//.WithExplicitStart();

var frontend = builder
    .AddViteApp("frontend", "../../frontend/apps/game-index", runScriptName: "vp run dev")
    .WithPnpm()
    .WithReference(playApi)
    .WaitFor(playApi)
    .WithReference(catalogApi)
    .WaitFor(catalogApi)
    .WithExplicitStart();

//api.PublishWithContainerFiles(frontend, "wwww");

var yarp = builder
    .AddYarp("frontend-server")
    .WithExternalHttpEndpoints()
    .PublishWithStaticFiles(frontend)
    .WithConfiguration(yarp =>
    {
        yarp.AddRoute("/api/games/{**catch-all}", catalogApi);
        yarp.AddRoute("/api/companies/{**catch-all}", catalogApi);
        yarp.AddRoute("/api/{**catch-all}", catalogApi);
    });

if (builder.Environment.IsDevelopment())
{
    yarp = yarp.WithHostPort(8081);
}

yarp.WithExplicitStart();

builder.Build().Run();
