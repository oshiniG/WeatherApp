using Polly;
using WeatherApp.Application.Interfaces;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services;
using WeatherApp.Infrastructure;
using WeatherApp.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.Configure<WeatherApiSettings>(builder.Configuration.GetSection("WeatherApi"));

// Add HttpClient for weather API
builder.Services.AddHttpClient<IWeatherApi, WeatherApi>()
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)))) 
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromMinutes(1))); 

// Add WeatherService (domain service)
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.Configure<WeatherApiSettings>(builder.Configuration.GetSection("WeatherApi"));

builder.Services.AddEndpointsApiExplorer();  
builder.Services.AddSwaggerGen(); 

var app = builder.Build();
app.UseMiddleware<RequestFailureMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  
    app.UseSwaggerUI(); 
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
