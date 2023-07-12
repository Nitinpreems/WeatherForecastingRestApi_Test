

using System.Text;
using System.Web;
using static WeatherForecast.Domain.Enums;

namespace WeatherForecast.Domain
{
    public static class ExtentionMethods
    {
        public static WeatherForecastDataModel MapResponseToDto(this WeatherServiceResponse weatherServiceResponse, string url)
        {
            var objWeatherForecastData = new WeatherForecastDataModel()
            {
                //Guid = new Guid(),
                Elevation = weatherServiceResponse.elevation,
                Generationtime_ms = weatherServiceResponse.generationtime_ms,
                Latitude = weatherServiceResponse.latitude,
                Longitude = weatherServiceResponse.longitude,
                Timezone = weatherServiceResponse.timezone,
                Timezone_abbreviation = weatherServiceResponse.timezone_abbreviation,
                Utc_offset_seconds = weatherServiceResponse.utc_offset_seconds,
                CreatedDate = DateTime.Now,
                Url = url,
                HourlyData = new List<HourlyData>(),
                DailyData = new List<DailyData>()
            };

            //Can be converted to less code using reflection
            if (weatherServiceResponse.hourly?.temperature_2m != null && weatherServiceResponse.hourly.temperature_2m.Count > 0)
            {
                var dataSet = new DataSet()
                {
                    //Guid = new Guid(),
                    DataSetName = Constants.hDsnTemperature_2m,
                    Unit = weatherServiceResponse.hourly_units.temperature_2m,
                    DataValues = new List<DataValue>()
                };

                int counter = 0;
                foreach (var time in weatherServiceResponse.hourly.time)
                {
                    dataSet.DataValues.Add(new DataValue()
                    {
                        Date = time,
                        Value = weatherServiceResponse.hourly.temperature_2m[counter]
                    });
                    counter++;
                }
                var hourlyData = new HourlyData()
                {
                    DataSets = new List<DataSet>(),
                    CreatedDate = DateTime.Now
                };
                hourlyData.DataSets.Add(dataSet);
                objWeatherForecastData.HourlyData.Add(hourlyData);
            }

            if (weatherServiceResponse.daily?.temperature_2m_max != null && weatherServiceResponse.daily.temperature_2m_max.Count > 0)
            {
                var dataSet = new DataSet()
                {
                    //Guid = new Guid(),
                    DataSetName = Constants.dDsnTemperature_2m_max,
                    Unit = weatherServiceResponse.daily_units.temperature_2m_max,
                    DataValues = new List<DataValue>()
                };

                int counter = 0;
                foreach (var time in weatherServiceResponse.daily.time)
                {
                    dataSet.DataValues.Add(new DataValue()
                    {
                        Date = time,
                        Value = weatherServiceResponse.daily.temperature_2m_max[counter]
                    });
                    counter++;
                }
                var dailyData = new DailyData()
                {
                    DataSets = new List<DataSet>(),
                    CreatedDate = DateTime.Now
                };
                dailyData.DataSets.Add(dataSet);
                objWeatherForecastData.DailyData.Add(dailyData);
            }

            return objWeatherForecastData;
        }

        public static string PrepareHttpUrl(this WeatherServiceRequest weatherServiceRequest)
        {
            StringBuilder serviceUrl = new StringBuilder($"{Constants.LatitudeQueryString}={weatherServiceRequest.Latitude}&{Constants.LongitudeQueryString}={weatherServiceRequest.Longitude}");

            if (weatherServiceRequest.HourlyDataSets == null && weatherServiceRequest.DailyDataSets == null)
            {
                serviceUrl.Append("&" + Constants.DefaultQueryString);
            }
            else
            {
                if (weatherServiceRequest.HourlyDataSets != null && weatherServiceRequest.HourlyDataSets.Count() > 0)
                {
                    StringBuilder hourlyDataSets = new StringBuilder(Constants.HourlyQueryString);
                    foreach (var dataset in weatherServiceRequest.HourlyDataSets)
                    {
                        hourlyDataSets.Append(dataset.ToString() + ",");
                    }
                    hourlyDataSets.Remove(hourlyDataSets.Length - 1, 1);
                    serviceUrl.Append("&" + hourlyDataSets);
                }
                if (weatherServiceRequest.DailyDataSets != null && weatherServiceRequest.DailyDataSets.Count() > 0)
                {
                    StringBuilder dailyDataSets = new StringBuilder(Constants.DailyQueryString);
                    foreach (var dataset in weatherServiceRequest.DailyDataSets)
                    {
                        dailyDataSets.Append(dataset.ToString() + ",");
                    }
                    dailyDataSets.Remove(dailyDataSets.Length - 1, 1);
                    string timeZone = (string.IsNullOrEmpty(weatherServiceRequest.Timezone.ToString())) ? Constants.DefaultTimezone : weatherServiceRequest.Timezone.GetTimeZoneString();
                    dailyDataSets.Append(Constants.TimezoneQueryString + "=" + timeZone);
                    serviceUrl.Append("&" + dailyDataSets);
                }
            }
            
            return serviceUrl.ToString();
        }
        
        public static string GetTimeZoneString(this TimeZoneEnum timeZoneEnumValue)
        {
            return timeZoneEnumValue.ToString().Replace("__","/");
        }
        public static string UpdateUrl(this string oldUrl, Dictionary<string, string> changedParam)
        {
            string newUrl = string.Empty;
            var UrlSplit = oldUrl?.Split("&");
            var dicUrlSplit = new Dictionary<string, string>(); 
            if (UrlSplit.Count() > 1)
                dicUrlSplit = UrlSplit.ToDictionary(s => s?.Split("=")?[0], s => s?.Split("=")?[1]);
            
            if (dicUrlSplit != null && dicUrlSplit.Any())
            {
                foreach (var item in changedParam)
                {
                    if (dicUrlSplit.ContainsKey(item.Key))
                    {
                        dicUrlSplit[item.Key] = item.Value;
                    }
                    else
                    {
                        dicUrlSplit.Add(item.Key, item.Value);
                    }
                }
                newUrl = string.Join("&", dicUrlSplit.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }
            newUrl = string.IsNullOrEmpty(newUrl) ? oldUrl : newUrl;
            return newUrl;
        }
    }
}
