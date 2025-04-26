using Microsoft.EntityFrameworkCore;
using WeatherApi.Models;

namespace WeatherApi.Data;

public static class DbSeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new WeatherDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<WeatherDbContext>>());

        // SQL:
        // SELECT CASE WHEN EXISTS (SELECT 1 FROM Locations) THEN 1 ELSE 0 END
        if (context.Locations.Any() || context.WeatherData.Any())
        {
            return; // DB has already been seeded
        }

        // Add seed data
        await SeedLocations(context);
        await SeedWeatherData(context);
    }

    private static async Task SeedLocations(WeatherDbContext context)
    {
        var locations = new List<Location>
        {
            new() { City = "New York", State = "NY", ZipCode = "10001", Country = "USA" },
            new() { City = "Los Angeles", State = "CA", ZipCode = "90001", Country = "USA" },
            new() { City = "Chicago", State = "IL", ZipCode = "60601", Country = "USA" },
            new() { City = "Houston", State = "TX", ZipCode = "77001", Country = "USA" },
            new() { City = "Phoenix", State = "AZ", ZipCode = "85001", Country = "USA" },
            new() { City = "Philadelphia", State = "PA", ZipCode = "19019", Country = "USA" },
            new() { City = "San Antonio", State = "TX", ZipCode = "78201", Country = "USA" },
            new() { City = "San Diego", State = "CA", ZipCode = "92101", Country = "USA" },
            new() { City = "Minneapolis", State = "MN", ZipCode = "55401", Country = "USA" },
            new() { City = "Saint Paul", State = "MN", ZipCode = "55101", Country = "USA" },
            new() { City = "Seattle", State = "WA", ZipCode = "98101", Country = "USA" },
            new() { City = "Boston", State = "MA", ZipCode = "02108", Country = "USA" },
            new() { City = "Miami", State = "FL", ZipCode = "33101", Country = "USA" },
            new() { City = "Denver", State = "CO", ZipCode = "80201", Country = "USA" },
            new() { City = "Atlanta", State = "GA", ZipCode = "30301", Country = "USA" },
            new() { City = "Portland", State = "OR", ZipCode = "97201", Country = "USA" },
            new() { City = "Las Vegas", State = "NV", ZipCode = "89101", Country = "USA" },
            new() { City = "Dallas", State = "TX", ZipCode = "75201", Country = "USA" },
            new() { City = "Detroit", State = "MI", ZipCode = "48201", Country = "USA" },
            new() { City = "St. Louis", State = "MO", ZipCode = "63101", Country = "USA" },
            new() { City = "Nashville", State = "TN", ZipCode = "37201", Country = "USA" },
            new() { City = "Baltimore", State = "MD", ZipCode = "21201", Country = "USA" },
            new() { City = "Louisville", State = "KY", ZipCode = "40201", Country = "USA" },
            new() { City = "Austin", State = "TX", ZipCode = "78701", Country = "USA" },
            new() { City = "Charlotte", State = "NC", ZipCode = "28201", Country = "USA" },
            new() { City = "Indianapolis", State = "IN", ZipCode = "46201", Country = "USA" },
            new() { City = "Columbus", State = "OH", ZipCode = "43201", Country = "USA" },
            new() { City = "San Francisco", State = "CA", ZipCode = "94101", Country = "USA" },
            new() { City = "Fort Worth", State = "TX", ZipCode = "76101", Country = "USA" },
            new() { City = "Jacksonville", State = "FL", ZipCode = "32201", Country = "USA" },
            new() { City = "San Jose", State = "CA", ZipCode = "95101", Country = "USA" },
            new() { City = "Oklahoma City", State = "OK", ZipCode = "73101", Country = "USA" },
            new() { City = "Memphis", State = "TN", ZipCode = "38101", Country = "USA" },
            new() { City = "Milwaukee", State = "WI", ZipCode = "53201", Country = "USA" },
            new() { City = "Kansas City", State = "MO", ZipCode = "64101", Country = "USA" },
            new() { City = "Sacramento", State = "CA", ZipCode = "94203", Country = "USA" },
            new() { City = "Raleigh", State = "NC", ZipCode = "27601", Country = "USA" },
            new() { City = "Omaha", State = "NE", ZipCode = "68101", Country = "USA" },
            new() { City = "Cleveland", State = "OH", ZipCode = "44101", Country = "USA" },
            new() { City = "Tulsa", State = "OK", ZipCode = "74101", Country = "USA" },
            new() { City = "Oakland", State = "CA", ZipCode = "94601", Country = "USA" },
            new() { City = "Wichita", State = "KS", ZipCode = "67201", Country = "USA" },
            new() { City = "Arlington", State = "TX", ZipCode = "76001", Country = "USA" },
            new() { City = "Bakersfield", State = "CA", ZipCode = "93301", Country = "USA" },
            new() { City = "Tampa", State = "FL", ZipCode = "33601", Country = "USA" },
            new() { City = "Aurora", State = "CO", ZipCode = "80010", Country = "USA" },
            new() { City = "Honolulu", State = "HI", ZipCode = "96801", Country = "USA" },
            new() { City = "Anaheim", State = "CA", ZipCode = "92801", Country = "USA" },
            new() { City = "Pittsburgh", State = "PA", ZipCode = "15201", Country = "USA" },
            new() { City = "Cincinnati", State = "OH", ZipCode = "45201", Country = "USA" },
            new() { City = "Anchorage", State = "AK", ZipCode = "99501", Country = "USA" },
            new() { City = "Toledo", State = "OH", ZipCode = "43601", Country = "USA" },
            new() { City = "Buffalo", State = "NY", ZipCode = "14201", Country = "USA" },
            new() { City = "Boise", State = "ID", ZipCode = "83701", Country = "USA" },
            new() { City = "Richmond", State = "VA", ZipCode = "23218", Country = "USA" },
            new() { City = "Newark", State = "NJ", ZipCode = "07101", Country = "USA" },
            new() { City = "Spokane", State = "WA", ZipCode = "99201", Country = "USA" },
            new() { City = "Albuquerque", State = "NM", ZipCode = "87101", Country = "USA" },
            new() { City = "Salt Lake City", State = "UT", ZipCode = "84101", Country = "USA" },
            new() { City = "Birmingham", State = "AL", ZipCode = "35201", Country = "USA" },
            new() { City = "Rochester", State = "NY", ZipCode = "14602", Country = "USA" },
            new() { City = "Fresno", State = "CA", ZipCode = "93701", Country = "USA" },
            new() { City = "Madison", State = "WI", ZipCode = "53701", Country = "USA" },
            new() { City = "Worcester", State = "MA", ZipCode = "01601", Country = "USA" },
            new() { City = "Des Moines", State = "IA", ZipCode = "50301", Country = "USA" },
            new() { City = "Tucson", State = "AZ", ZipCode = "85701", Country = "USA" },
            new() { City = "Providence", State = "RI", ZipCode = "02901", Country = "USA" },
            new() { City = "Little Rock", State = "AR", ZipCode = "72201", Country = "USA" },
            new() { City = "Charleston", State = "SC", ZipCode = "29401", Country = "USA" },
            new() { City = "Syracuse", State = "NY", ZipCode = "13201", Country = "USA" },
            new() { City = "Fort Wayne", State = "IN", ZipCode = "46801", Country = "USA" },
            new() { City = "Dayton", State = "OH", ZipCode = "45401", Country = "USA" },
            new() { City = "Albany", State = "NY", ZipCode = "12201", Country = "USA" },
            new() { City = "Grand Rapids", State = "MI", ZipCode = "49501", Country = "USA" },
            new() { City = "Springfield", State = "MA", ZipCode = "01101", Country = "USA" },
            new() { City = "Shreveport", State = "LA", ZipCode = "71101", Country = "USA" },
            new() { City = "Mobile", State = "AL", ZipCode = "36601", Country = "USA" },
            new() { City = "Knoxville", State = "TN", ZipCode = "37901", Country = "USA" },
            new() { City = "Tacoma", State = "WA", ZipCode = "98401", Country = "USA" },
            new() { City = "Reno", State = "NV", ZipCode = "89501", Country = "USA" },
            new() { City = "Montgomery", State = "AL", ZipCode = "36101", Country = "USA" },
            new() { City = "Huntsville", State = "AL", ZipCode = "35801", Country = "USA" },
            new() { City = "Lexington", State = "KY", ZipCode = "40501", Country = "USA" },
            new() { City = "Akron", State = "OH", ZipCode = "44301", Country = "USA" },
            new() { City = "Stockton", State = "CA", ZipCode = "95201", Country = "USA" },
            new() { City = "Orlando", State = "FL", ZipCode = "32801", Country = "USA" },
            new() { City = "Corpus Christi", State = "TX", ZipCode = "78401", Country = "USA" },
            new() { City = "Baton Rouge", State = "LA", ZipCode = "70801", Country = "USA" },
            new() { City = "Winston-Salem", State = "NC", ZipCode = "27101", Country = "USA" },
            new() { City = "Greensboro", State = "NC", ZipCode = "27401", Country = "USA" },
            new() { City = "Durham", State = "NC", ZipCode = "27701", Country = "USA" },
            new() { City = "Laredo", State = "TX", ZipCode = "78040", Country = "USA" },
            new() { City = "Lubbock", State = "TX", ZipCode = "79401", Country = "USA" },
            new() { City = "Chesapeake", State = "VA", ZipCode = "23320", Country = "USA" },
            new() { City = "Irving", State = "TX", ZipCode = "75060", Country = "USA" },
            new() { City = "Garland", State = "TX", ZipCode = "75040", Country = "USA" },
            new() { City = "Glendale", State = "AZ", ZipCode = "85301", Country = "USA" },
            new() { City = "Hialeah", State = "FL", ZipCode = "33010", Country = "USA" },
            new() { City = "Scottsdale", State = "AZ", ZipCode = "85251", Country = "USA" },
            new() { City = "Norfolk", State = "VA", ZipCode = "23501", Country = "USA" },
            new() { City = "Jersey City", State = "NJ", ZipCode = "07302", Country = "USA" },
            new() { City = "Chandler", State = "AZ", ZipCode = "85225", Country = "USA" },
            new() { City = "Henderson", State = "NV", ZipCode = "89002", Country = "USA" },
            new() { City = "North Las Vegas", State = "NV", ZipCode = "89030", Country = "USA" },
            new() { City = "Chula Vista", State = "CA", ZipCode = "91910", Country = "USA" },
            new() { City = "Gilbert", State = "AZ", ZipCode = "85233", Country = "USA" },
            new() { City = "Santa Ana", State = "CA", ZipCode = "92701", Country = "USA" },
            new() { City = "Fort Lauderdale", State = "FL", ZipCode = "33301", Country = "USA" },
            new() { City = "Tempe", State = "AZ", ZipCode = "85281", Country = "USA" },
            new() { City = "Ontario", State = "CA", ZipCode = "91761", Country = "USA" },
            new() { City = "Vancouver", State = "WA", ZipCode = "98660", Country = "USA" },
            new() { City = "Springfield", State = "MO", ZipCode = "65801", Country = "USA" },
            new() { City = "Pembroke Pines", State = "FL", ZipCode = "33023", Country = "USA" },
            new() { City = "Salem", State = "OR", ZipCode = "97301", Country = "USA" },
            new() { City = "Lancaster", State = "CA", ZipCode = "93534", Country = "USA" },
            new() { City = "Eugene", State = "OR", ZipCode = "97401", Country = "USA" },
            new() { City = "Peoria", State = "AZ", ZipCode = "85345", Country = "USA" }
        };

        // SQL:
        // INSERT INTO Locations (City, State, ZipCode, Country)
        // VALUES
        // ('New York', 'NY', '10001', 'USA'),
        // ('Los Angeles', 'CA', '90001', 'USA'),
        // ...
        // ('Peoria', 'AZ', '85345', 'USA');
        await context.Locations.AddRangeAsync(locations);
        await context.SaveChangesAsync();
    }

    private static async Task SeedWeatherData(WeatherDbContext context)
    {
        // Get all location IDs
        // SQL:
        // SELECT Id FROM Locations
        var locationIds = await context.Locations.Select(l => l.Id).ToListAsync();
        var random = new Random();
        var weatherData = new List<WeatherData>();
        var now = DateTime.UtcNow;

        // Generate weather data for the past 100 days for each location
        foreach (var locationId in locationIds)
        {
            for (int i = 0; i < 100; i++)
            {
                // Random temperature in Fahrenheit between 0°F and 105°F
                var temperature = random.NextDouble() * 105;

                // Date is current date minus i days
                var recordedAt = now.Date.AddDays(-i);

                weatherData.Add(new WeatherData
                {
                    LocationId = locationId,
                    Temperature = Math.Round(temperature, 1), // Round to 1 decimal place
                    RecordedAt = recordedAt
                });
            }
        }

        // SQL:
        // INSERT INTO WeatherData (LocationId, Temperature, RecordedAt)
        // VALUES
        // ({locationId1}, {temperature1}, {recordedAt1}),
        // ({locationId2}, {temperature2}, {recordedAt2}),
        await context.WeatherData.AddRangeAsync(weatherData);
        await context.SaveChangesAsync();
    }
}