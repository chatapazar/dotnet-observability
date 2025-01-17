using Prometheus;
using Sample.Web;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();


// This sample demonstrates how to integrate prometheus-net into a web app.
// 
// NuGet packages required:
// * prometheus-net.AspNetCore
// * prometheus-net.AspNetCore.HealthChecks (optional; for publishing health check results)

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();

// Define an HTTP client that reports metrics about its usage, to be used by a sample background service.
//builder.Services.AddHttpClient(SampleService.HttpClientName);

// Export metrics from all HTTP clients registered in services
builder.Services.UseHttpClientMetrics();

// A sample service that uses the above HTTP client.
//builder.Services.AddHostedService<SampleService>();

builder.Services.AddHealthChecks()
    // Define a sample health check that always signals healthy state.
    .AddCheck<SampleHealthCheck>(nameof(SampleHealthCheck))
    // Report health check results in the metrics output.
    .ForwardToPrometheus();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles();

app.UseRouting();

// Capture metrics about all received HTTP requests.
app.UseHttpMetrics();

app.UseAuthorization();

//app.MapRazorPages();
app.MapGet("/", () => "Hello World!");

app.MapHealthChecks("/healthz");

app.MapMetrics();


app.Run();