using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WeatherApi.Data;
using WeatherApi.Endpoints.Locations;
using WeatherApi.Endpoints.Reports;
using WeatherApi.Endpoints.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = null;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Configure DbContext for Azure SQL with local identity
string connectionString = builder.Configuration.GetConnectionString("AzureSqlConnection")
    ?? throw new InvalidOperationException("Connection string 'AzureSqlConnection' not found.");

// Configure Azure SQL with DefaultAzureCredential for local identity
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Ensure database is created and then seed if needed
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<WeatherDbContext>();
            // Create database if it doesn't exist
            context.Database.EnsureCreated();
            // Seed the database
            await DbSeedData.Initialize(services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while creating/seeding the database.");
        }
    }
}

// Use CORS middleware
app.UseCors();

// Register all endpoints
app.MapLocationEndpoints();
app.MapWeatherEndpoints();
app.MapReportEndpoints();

app.Run();
