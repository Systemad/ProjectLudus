using Projects;

var builder = DistributedApplication.CreateBuilder(args);
var apiService = builder.AddProject<API>("apiservice");


builder.AddProject<Client>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();