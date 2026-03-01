var builder = DistributedApplication.CreateBuilder(args);

var pgUsername = builder.AddParameter("pg-username", "postgres", secret: false);
var pgPassword = builder.AddParameter("pg-password", "PQDF13*7dpR-Q77nmQZh*3", secret: true);

var catalogDb = builder
    .AddPostgres(name: "catalog-primary", pgUsername, pgPassword)
    .WithHostPort(5433)
    .WithImage(image: "paradedb/paradedb", tag: "v0.21.0-pg17")
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

builder.AddProject<Projects.CatalogAPI>("catalogapi").WaitFor(catalogDb).WithReference(catalogDb);

builder.Build().Run();
