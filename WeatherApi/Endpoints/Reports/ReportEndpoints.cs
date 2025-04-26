using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;

namespace WeatherApi.Endpoints.Reports;

public static class ReportEndpoints
{
    public static void MapReportEndpoints(this WebApplication app)
    {
        // Average temperature by location
        app.MapGet("/api/reports/average-by-location", async (WeatherDbContext db) =>
        {
            // SQL:
            // SELECT
            //     l.Id AS LocationId, l.City, l.State, l.ZipCode, l.Country,
            //     ROUND(AVG(w.Temperature), 1) AS AverageTemperature,
            //     ROUND(MIN(w.Temperature), 1) AS MinTemperature,
            //     ROUND(MAX(w.Temperature), 1) AS MaxTemperature,
            //     COUNT(w.Id) AS ReadingsCount
            // FROM Locations l
            // LEFT JOIN WeatherData w ON l.Id = w.LocationId
            // GROUP BY l.Id, l.City, l.State, l.ZipCode, l.Country
            // ORDER BY AVG(w.Temperature) DESC
            var averageTemps = await db.Locations
                .Select(location => new
                {
                    LocationId = location.Id,
                    City = location.City,
                    State = location.State,
                    ZipCode = location.ZipCode,
                    Country = location.Country,
                    AverageTemperature = Math.Round(location.WeatherData.Average(w => w.Temperature), 1),
                    MinTemperature = Math.Round(location.WeatherData.Min(w => w.Temperature), 1),
                    MaxTemperature = Math.Round(location.WeatherData.Max(w => w.Temperature), 1),
                    ReadingsCount = location.WeatherData.Count()
                })
                .OrderByDescending(l => l.AverageTemperature)
                .ToListAsync();

            return Results.Ok(new
            {
                ReportName = "Average Temperatures by Location",
                GeneratedAt = DateTime.UtcNow,
                Data = averageTemps
            });
        });

        // Top 10 hottest locations based on average temperature
        app.MapGet("/api/reports/hottest-locations", async (WeatherDbContext db) =>
        {
            // SQL:
            // SELECT
            //     l.City, l.State, l.Country,
            //     ROUND(AVG(w.Temperature), 1) AS AverageTemperature
            // FROM Locations l
            // JOIN WeatherData w ON l.Id = w.LocationId
            // GROUP BY l.City, l.State, l.Country
            // ORDER BY AVG(w.Temperature) DESC
            // LIMIT 10
            var hottestLocations = await db.Locations
                .Select(location => new
                {
                    City = location.City,
                    State = location.State,
                    Country = location.Country,
                    AverageTemperature = Math.Round(location.WeatherData.Average(w => w.Temperature), 1)
                })
                .OrderByDescending(l => l.AverageTemperature)
                .Take(10)
                .ToListAsync();

            return Results.Ok(new
            {
                ReportName = "Top 10 Hottest Locations",
                GeneratedAt = DateTime.UtcNow,
                Data = hottestLocations
            });
        });

        // Top 10 coldest locations based on average temperature
        app.MapGet("/api/reports/coldest-locations", async (WeatherDbContext db) =>
        {
            // SQL:
            // SELECT
            //     l.City, l.State, l.Country,
            //     ROUND(AVG(w.Temperature), 1) AS AverageTemperature
            // FROM Locations l
            // JOIN WeatherData w ON l.Id = w.LocationId
            // GROUP BY l.City, l.State, l.Country
            // ORDER BY AVG(w.Temperature) ASC
            // LIMIT 10
            var coldestLocations = await db.Locations
                .Select(location => new
                {
                    City = location.City,
                    State = location.State,
                    Country = location.Country,
                    AverageTemperature = Math.Round(location.WeatherData.Average(w => w.Temperature), 1)
                })
                .OrderBy(l => l.AverageTemperature)
                .Take(10)
                .ToListAsync();

            return Results.Ok(new
            {
                ReportName = "Top 10 Coldest Locations",
                GeneratedAt = DateTime.UtcNow,
                Data = coldestLocations
            });
        });

        // Temperature trends over time (last 7 days)
        app.MapGet("/api/reports/temperature-trends", async (WeatherDbContext db) =>
        {
            // Get current date
            var today = DateTime.UtcNow.Date;

            // SQL:
            // SELECT
            //     DATE(w.RecordedAt) AS Date,
            //     ROUND(AVG(w.Temperature), 1) AS AverageTemperature,
            //     COUNT(w.Id) AS RecordCount
            // FROM WeatherData w
            // WHERE w.RecordedAt >= DATE_SUB(CURRENT_DATE(), INTERVAL 7 DAY)
            // GROUP BY DATE(w.RecordedAt)
            // ORDER BY Date ASC
            var trends = await db.WeatherData
                .Where(w => w.RecordedAt >= today.AddDays(-7))
                .GroupBy(w => w.RecordedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    AverageTemperature = Math.Round(g.Average(w => w.Temperature), 1),
                    RecordCount = g.Count()
                })
                .OrderBy(t => t.Date)
                .ToListAsync();

            return Results.Ok(new
            {
                ReportName = "Daily Temperature Trends (Last 7 Days)",
                GeneratedAt = DateTime.UtcNow,
                Data = trends
            });
        });
    }
}