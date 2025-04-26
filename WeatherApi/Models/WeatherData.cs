namespace WeatherApi.Models;

public class WeatherData
{
    public int Id { get; set; }
    public double Temperature { get; set; }
    public DateTime RecordedAt { get; set; }

    // Foreign key to Location
    public int LocationId { get; set; }

    // Navigation property
    public Location? Location { get; set; }
}