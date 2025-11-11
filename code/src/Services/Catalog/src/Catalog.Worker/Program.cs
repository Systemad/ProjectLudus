using Catalog.Worker;
using ServiceDefaults;


// TODO: THIS PROJECT DOES THE FOLLOWING
// DOES THE HEAVY ETL JOB (TRIGGERED MANUALLY!!!!)

// NORMAL
// LISTENS TO QUEUE, FROM INGESTER, FETCHED DATA, TRANSFORMS AND INSERT INTO THE DATABASE
var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();