using Aspire.Hosting.Docker.Resources.ServiceNodes.Swarm;
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

var typesenseMasterKey = builder.AddParameter("TYPESENSE-ADMIN-KEY");

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
    .WithBindMount("../../typesense-data", "/data")
    .WithArgs(
        "--data-dir",
        "/data",
        // "--api-key=typesense-index-gaming",
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

var api = builder.AddProject<Projects.PlayAPI>("playapi").WaitFor(playDb).WithReference(playDb);
//.WithReference(typesense)
//.WithExplicitStart();

var frontend = builder
    .AddViteApp("frontend", "../../web/apps/game-index", runScriptName: "dev2")
    .WithPnpm()
    .WithReference(api)
    .WaitFor(api)
    .WithExplicitStart();
//api.PublishWithContainerFiles(frontend, "wwww");

builder
    .AddYarp("frontend-server")
    .WithExternalHttpEndpoints()
    .PublishWithStaticFiles(frontend)
    .WithConfiguration(yarp =>
    {
        // Always proxy /api requests to backend
        yarp.AddRoute("/api/{**catch-all}", api);
    })
    .WithExplicitStart();

builder.Build().Run();
