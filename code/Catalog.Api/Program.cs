using BuildingBlocks.OpenApi;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructure();

//builder.AddServiceDefaults();

/*
 *
 * builder.Services.AddFastEndpoints(
    o => o.Assemblies = new[]
    {
        typeof(SomeAssemblyName).Assembly,
        typeof(AnotherAssemblyName).Assembly
    });
 */





builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();



//builder.Logging.AddConsole();
/*
builder.Services.AddTickerQ(options =>
{
    options.SetMaxConcurrency(4);
    //options.AddDashboard();
    //options.SetExceptionHandler<MyExceptionHandler>();
});
*/
// builder.Services.AddHostedService<SyncWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseAspnetOpenApi();
}
app.UseDefaultExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.UseFastEndpoints(x =>
    {
        x.Errors.UseProblemDetails();
        //x.Endpoints.ShortNames = true;
    })
    .UseSwaggerGen(c => { });

//app.UseTickerQ(TickerQStartMode.Immediate);
//ITickerHost tickerHost = app.Services.GetRequiredService<ITickerHost>();
//tickerHost.Start();

await app.RunAsync();
