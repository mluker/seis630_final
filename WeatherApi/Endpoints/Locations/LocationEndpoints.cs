using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;
using WeatherApi.Models;

namespace WeatherApi.Endpoints.Locations;

public static class LocationEndpoints
{
    public static void MapLocationEndpoints(this WebApplication app)
    {
        // Get all locations
        app.MapGet("/api/locations", async (WeatherDbContext db) =>
            // SQL:
            // SELECT * FROM Locations
            await db.Locations.ToListAsync());

        // Get location by id
        app.MapGet("/api/locations/{id}", async (int id, WeatherDbContext db) =>
        {
            // SQL:
            // SELECT * FROM Locations WHERE Id = {id}
            var locationData = await db.Locations.FindAsync(id);

            return locationData != null
                ? Results.Ok(locationData)
                : Results.NotFound();
        });

        // Get weather data for a specific location
        app.MapGet("/api/locations/{id}/weather", async (int id, WeatherDbContext db) =>
        {
            // SQL:
            // SELECT * FROM WeatherData
            // WHERE LocationId = {id}
            var weatherData = await db.WeatherData
                .Where(w => w.LocationId == id)
                .ToListAsync();

            return weatherData.Any()
                ? Results.Ok(weatherData)
                : Results.NotFound();
        });

        // Add a new location
        app.MapPost("/api/locations", async (Location location, WeatherDbContext db) =>
        {
            // Check if location already exists (based on city, state, zip and country)
            // SQL:
            // SELECT * FROM Locations
            // WHERE City = {location.City}
            // AND State = {location.State}
            // AND ZipCode = {location.ZipCode}
            // AND Country = {location.Country}
            // LIMIT 1
            var existingLocation = await db.Locations.FirstOrDefaultAsync(l =>
                l.City == location.City &&
                l.State == location.State &&
                l.ZipCode == location.ZipCode &&
                l.Country == location.Country);

            // Return existing location if found
            if (existingLocation != null)
            {
                return Results.Ok(new
                {
                    Message = "Location already exists",
                    Location = existingLocation
                });
            }

            // Create new location if none exists
            // SQL:
            // INSERT INTO Locations (City, State, ZipCode, Country)
            // VALUES ({location.City}, {location.State}, {location.ZipCode}, {location.Country})
            db.Locations.Add(location);
            await db.SaveChangesAsync();
            return Results.Created($"/api/locations/{location.Id}", location);
        });

        // Delete location by id
        app.MapDelete("/api/locations/{id}", async (int id, bool cascade, WeatherDbContext db) =>
        {
            // Find the location
            // SQL:
            // SELECT * FROM Locations WHERE Id = {id}
            var location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return Results.NotFound();
            }

            // Check if location has related weather data
            // SQL:
            // SELECT CASE WHEN EXISTS(
            //   SELECT 1 FROM WeatherData WHERE LocationId = {id}
            // ) THEN 1 ELSE 0 END
            var hasRelatedData = await db.WeatherData.AnyAsync(w => w.LocationId == id);

            if (hasRelatedData && !cascade)
            {
                // Return error if location has related data and cascade is false
                return Results.BadRequest(new
                {
                    Error = "Location has related weather data. Set cascade=true to delete the location and its related data."
                });
            }

            if (hasRelatedData && cascade)
            {
                // Delete all related weather data first
                // SQL:
                // SELECT * FROM WeatherData WHERE LocationId = {id}
                var relatedWeatherData = await db.WeatherData
                    .Where(w => w.LocationId == id)
                    .ToListAsync();

                // SQL:
                // DELETE FROM WeatherData WHERE LocationId = {id}
                db.WeatherData.RemoveRange(relatedWeatherData);
                await db.SaveChangesAsync();
            }

            // Delete the location
            // SQL:
            // DELETE FROM Locations WHERE Id = {id}
            db.Locations.Remove(location);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}