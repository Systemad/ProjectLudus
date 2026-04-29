#:sdk Aspire.AppHost.Sdk@13.2.4
#:package Aspire.Hosting.PostgreSQL@13.2.4
#:package Aspire.Hosting.Yarp@13.2.4
#:package Aspire.Hosting.JavaScript@13.2.4
#:package Aspire.Hosting.Docker@13.2.4
#:package Aspire.Hosting.Python@13.2.4

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

var pgUsername = builder.AddParameter("pg-username", "postgres", secret: false);
var pgPassword = builder.AddParameter("pg-password", "pudGW.E7_u8eF8Qhnym)E0", secret: true);

var postgres = builder
    .AddPostgres(name: "main-postgres", pgUsername, pgPassword)
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

builder
    .AddContainer("typesense", "typesense/typesense:30.2.rc12")
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
    )
    .WithLifetime(ContainerLifetime.Session);

var playApi = builder
    .AddProject("playapi", "./backend/PlayAPI")
    .WaitFor(playDb)
    .WithReference(playDb)
    .WithHttpsEndpoint();

//.WithExternalHttpEndpoints();

var catalogApi = builder
    .AddProject("catalogApi", "./backend/CatalogAPI")
    .WaitFor(catalogDb)
    .WithReference(catalogDb)
    .WithHttpsEndpoint();

//.WithExternalHttpEndpoints();

var frontend = builder
    .AddJavaScriptApp("frontend", "frontend/apps/game-index", runScriptName: "dev")
    .WithPnpm()
    .WithReference(playApi)
    .WaitFor(playApi)
    .WithReference(catalogApi)
    .WaitFor(catalogApi);

#pragma warning disable ASPIRECERTIFICATES001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
builder
    .AddYarp("frontend-server")
    .WithHttpsEndpoint()
    .WithHttpsDeveloperCertificate()
    .WithConfiguration(yarp =>
    {
        yarp.AddRoute("/catalog/{**catch-all}", catalogApi);
        yarp.AddRoute("/play/{**catch-all}", playApi);
    })
    .WithHostPort(53489)
    .WithExternalHttpEndpoints()
    .WithLifetime(ContainerLifetime.Session)
    .PublishWithStaticFiles(frontend);
#pragma warning restore ASPIRECERTIFICATES001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

//.WithExternalHttpEndpoints();
//.PublishWithStaticFiles(frontend);

builder.Build().Run();
