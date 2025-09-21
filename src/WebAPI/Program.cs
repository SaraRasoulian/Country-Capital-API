using Application.Contract.Countries;
using Application.Contract.Shared;
using Application.Shared;
using Polly;
using Polly.Extensions.Http;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Dependency Injection
builder.Services.AddScoped<ICountryApplication, CountryApplication>();
builder.Services.AddScoped<ISoapHelper, SoapHelper>();

// Retry and Timeout
var soapSettings = builder.Configuration.GetSection("SoapSettings");

builder.Services.AddHttpClient("SoapClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(int.Parse(soapSettings["TimeoutSeconds"]));
}).AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .Or<TaskCanceledException>() // timeouts
    .WaitAndRetryAsync(
        retryCount: int.Parse(soapSettings["RetryCount"]),
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // 2s, 4s, 8s
        onRetry: (outcome, timespan, attempt, context) =>
        {
            logger.Warning($"Retry {attempt} after {timespan.TotalSeconds}s due to: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
        }));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global exception handler
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var contextFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            logger.Error(contextFeature.Error, "Unhandled exception caught.");
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = contextFeature.Error.Message,
                Type = contextFeature.Error.GetType().Name
            });
        }
    });
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
