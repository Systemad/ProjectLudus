using BuildingBlocks.FastEndpoint;
using BuildingBlocks.OpenApi;
using Catalog;
using Catalog.Extensions;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommonInfrastructure();
builder.AddDatabaseInfrastructure();
builder.AddFastEndpoints<CatalogRoot>("Catalog API");




/*
builder.Services.AddTickerQ(options =>
{
    options.SetMaxConcurrency(4);
    //options.AddDashboard();
    //options.SetExceptionHandler<MyExceptionHandler>();
});
*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseAspnetOpenApi();
}
app.UseDefaultExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();

app.UseFastEndpointsExt();
//app.UseTickerQ(TickerQStartMode.Immediate);
//ITickerHost tickerHost = app.Services.GetRequiredService<ITickerHost>();
//tickerHost.Start();

await app.RunAsync();
