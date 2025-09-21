using Application.Contract.Countries;
using Application.Contract.Shared;
using Application.Shared;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddScoped<ICountryApplication, CountryApplication>();
builder.Services.AddScoped<ISoapHelper, SoapHelper>();

#region Retry and Timeout

var soapSettings = builder.Configuration.GetSection("SoapSettings");

// Add HttpClient with Polly policies
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
            Console.WriteLine($"Retry {attempt} after {timespan.TotalSeconds}s due to: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
        }));
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
