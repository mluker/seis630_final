namespace WeatherApi.Models;

public class Location
{
    public int Id { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Navigation property for related weather data
    public ICollection<WeatherData> WeatherData { get; set; } = new List<WeatherData>();
}