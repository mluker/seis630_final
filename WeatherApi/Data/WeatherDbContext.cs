using Microsoft.EntityFrameworkCore;
using WeatherApi.Models;

namespace WeatherApi.Data;

public class WeatherDbContext : DbContext
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
        : base(options)
    {
    }

    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<WeatherData> WeatherData { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the relationship between WeatherData and Location
        modelBuilder.Entity<WeatherData>()
            .HasOne(w => w.Location)
            .WithMany(l => l.WeatherData)
            .HasForeignKey(w => w.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void EnsureDatabaseCreated()
    {
        Database.EnsureCreated();
    }
}