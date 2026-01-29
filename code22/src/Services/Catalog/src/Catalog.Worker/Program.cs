using Catalog.Extensions;
using Catalog.Worker;
using ServiceDefaults;

// https://auth0.com/blog/authentication-authorization-enhancements-dotnet-10/
// TODO: THIS PROJECT DOES THE FOLLOWING
// DOES THE HEAVY ETL JOB (TRIGGERED MANUALLY!!!!)

// NORMAL
// LISTENS TO QUEUE, FROM INGESTER, FETCHED DATA, TRANSFORMS AND INSERT INTO THE DATABASE
var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddDatabaseInfrastructure();

//builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();