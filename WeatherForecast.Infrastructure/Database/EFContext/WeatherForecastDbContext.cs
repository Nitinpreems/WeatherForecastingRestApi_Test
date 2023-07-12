using Microsoft.EntityFrameworkCore;
using WeatherForecast.Domain;

namespace WeatherForecast.Infrastructure
{
    public class WeatherForecastDbContext : DbContext
    {
        public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) : base(options) { }

        public WeatherForecastDbContext() { }

        public DbSet<WeatherForecastDataModel> WeatherForecastData { get; set; }
        public DbSet<HourlyData> HourlyData { get; set; }
        public DbSet<DailyData> DailyData { get; set; }
        public DbSet<DataSet> DataSet { get; set; }
        public DbSet<DataValue> DataValue { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<WeatherForecastDataModel>().HasMany(x => x.HourlyData);
        //    modelBuilder.Entity<HourlyData>().HasMany(x => x.DataSets);
        //}

    }
}
