using Application.Contract.Countries;
using Application.Contract.Shared;
using Application.Shared;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddScoped<ICountryApplication, CountryApplication>();
builder.Services.AddScoped<ISoapHelper, SoapHelper>();

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
