

using static WeatherForecast.Domain.Enums;

namespace WeatherForecast.Domain
{
    public class WeatherServiceRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public TimeZoneEnum Timezone { get; set; }
        public List<WeatherHourlyDataSet>? HourlyDataSets { get; set; }
        public List<WeatherDailyDataSet>? DailyDataSets { get; set; }
        
    }
    
}