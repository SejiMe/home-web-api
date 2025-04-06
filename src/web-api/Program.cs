using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using web.api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuth();

builder.Services.Configure<ConnectionStringsOptions>(
    builder.Configuration.GetSection(ConnectionStringsOptions.HomeDb)
);
builder.Configuration.AddUserSecrets<Program>(true, true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCommon(builder.Configuration);
builder.Services.AddEfCore(builder.Configuration);

builder.Services.AddServices();

var app = builder.Build();

app.RegisterTodoEndpoint();
app.RegisterCategoryEndpoint();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.Title = "Home API";

        options.DarkMode = true;
        options.Theme = ScalarTheme.Moon;
        options.OpenApiRoutePattern = "/openapi/v1.json";
    });
}

var summaries = new[]
{
    "Freezing",
    "Bracing",
    "Chilly",
    "Cool",
    "Mild",
    "Warm",
    "Balmy",
    "Hot",
    "Sweltering",
    "Scorching",
};

app.MapGet(
        "/weatherforecast",
        () =>
        {
            var forecast = Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        }
    )
    .RequireAuthorization()
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
