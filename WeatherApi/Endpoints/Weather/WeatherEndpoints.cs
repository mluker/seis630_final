using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;
using WeatherApi.Models;

namespace WeatherApi.Endpoints.Weather;

public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this WebApplication app)
    {
        // Get all weather data with full location details
        app.MapGet("/api/weather", async (WeatherDbContext db) =>
        {
            // SQL:
            // SELECT w.*, l.*
            // FROM WeatherData w
            // LEFT JOIN Locations l ON w.LocationId = l.Id
            var weatherData = await db.WeatherData
                .Include(w => w.Location)
                .ToListAsync();

            // Transform data to include full location details without redundant locationId
            var result = weatherData.Select(w => new
            {
                Id = w.Id,
                Temperature = w.Temperature,
                RecordedAt = w.RecordedAt,
                Location = w.Location != null ? new
                {
                    Id = w.Location.Id,
                    City = w.Location.City,
                    State = w.Location.State,
                    ZipCode = w.Location.ZipCode,
                    Country = w.Location.Country
                } : null
            }).ToList();

            return Results.Ok(result);
        });

        // Get weather data by id with full location details
        app.MapGet("/api/weather/{id}", async (int id, WeatherDbContext db) =>
        {
            // SQL:
            // SELECT w.*, l.*
            // FROM WeatherData w
            // LEFT JOIN Locations l ON w.LocationId = l.Id
            // WHERE w.Id = {id}
            // LIMIT 1
            var weatherData = await db.WeatherData
                .Include(w => w.Location)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (weatherData == null)
                return Results.NotFound();

            // Transform data to include full location details without redundant locationId
            var result = new
            {
                Id = weatherData.Id,
                Temperature = weatherData.Temperature,
                RecordedAt = weatherData.RecordedAt,
                Location = weatherData.Location != null ? new
                {
                    Id = weatherData.Location.Id,
                    City = weatherData.Location.City,
                    State = weatherData.Location.State,
                    ZipCode = weatherData.Location.ZipCode,
                    Country = weatherData.Location.Country
                } : null
            };

            return Results.Ok(result);
        });

        app.MapPost("/api/weather", async (WeatherData weatherData, WeatherDbContext db) =>
        {
            // Weather with LocationId only (existing location)
            if (weatherData.LocationId > 0 && weatherData.Location == null)
            {
                // SQL:
                // SELECT * FROM Locations WHERE Id = {weatherData.LocationId}
                var location = await db.Locations.FindAsync(weatherData.LocationId);
                if (location == null)
                {
                    return Results.BadRequest($"Location with ID {weatherData.LocationId} does not exist.");
                }

                // SQL:
                // INSERT INTO WeatherData (Temperature, RecordedAt, LocationId)
                // VALUES ({weatherData.Temperature}, {weatherData.RecordedAt}, {weatherData.LocationId})
                db.WeatherData.Add(weatherData);
                await db.SaveChangesAsync();
            }
            // Weather with new Location object
            else if (weatherData.Location != null)
            {
                // SQL:
                // SELECT * FROM Locations
                // WHERE City = {weatherData.Location.City}
                // AND State = {weatherData.Location.State}
                // AND ZipCode = {weatherData.Location.ZipCode}
                // AND Country = {weatherData.Location.Country}
                // LIMIT 1
                var existingLocation = await db.Locations.FirstOrDefaultAsync(l =>
                    l.City == weatherData.Location.City &&
                    l.State == weatherData.Location.State &&
                    l.ZipCode == weatherData.Location.ZipCode &&
                    l.Country == weatherData.Location.Country);

                if (existingLocation != null)
                {
                    // Use existing location
                    weatherData.LocationId = existingLocation.Id;
                    weatherData.Location = null;
                }

                // SQL (if Location is null):
                // INSERT INTO WeatherData (Temperature, RecordedAt, LocationId)
                // VALUES ({weatherData.Temperature}, {weatherData.RecordedAt}, {weatherData.LocationId})
                //
                // SQL (if Location is provided):
                // -- First inserts the new location:
                // INSERT INTO Locations (City, State, ZipCode, Country)
                // VALUES ({weatherData.Location.City}, {weatherData.Location.State}, {weatherData.Location.ZipCode}, {weatherData.Location.Country});
                // -- Then inserts the weather data with the new location ID:
                // INSERT INTO WeatherData (Temperature, RecordedAt, LocationId)
                // VALUES ({weatherData.Temperature}, {weatherData.RecordedAt}, {locationId})
                db.WeatherData.Add(weatherData);
                await db.SaveChangesAsync();
            }
            else
            {
                return Results.BadRequest("Either LocationId or Location details must be provided.");
            }

            // SQL:
            // SELECT l.* FROM Locations l
            // JOIN WeatherData w ON l.Id = w.LocationId
            // WHERE w.Id = {weatherData.Id}
            await db.Entry(weatherData).Reference(w => w.Location).LoadAsync();

            var result = new
            {
                Id = weatherData.Id,
                Temperature = weatherData.Temperature,
                RecordedAt = weatherData.RecordedAt,
                Location = weatherData.Location != null ? new
                {
                    Id = weatherData.Location.Id,
                    City = weatherData.Location.City,
                    State = weatherData.Location.State,
                    ZipCode = weatherData.Location.ZipCode,
                    Country = weatherData.Location.Country
                } : null
            };

            return Results.Created($"/api/weather/{weatherData.Id}", result);
        });

        // Delete weather data by id
        app.MapDelete("/api/weather/{id}", async (int id, WeatherDbContext db) =>
        {
            // SQL:
            // SELECT * FROM WeatherData WHERE Id = {id}
            var weatherData = await db.WeatherData.FindAsync(id);

            if(weatherData == null)
                return Results.NotFound();

            // SQL:
            // DELETE FROM WeatherData WHERE Id = {id}
            db.WeatherData.Remove(weatherData);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}