
using Microsoft.Extensions.Logging;
using WeatherForecast.Domain;

namespace WeatherForecast.Infrastructure
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientsFactory _httpClientsFactory;
        private readonly ILogger<HttpClientsFactory> _logger;
        
        public WeatherService(IHttpClientsFactory httpClientsFactory, ILogger<HttpClientsFactory> logger)
        {
            _httpClientsFactory = httpClientsFactory;
            _logger = logger;
        }

        
        public async Task<WeatherServiceResponse> GetWeatherForecastData(string serviceUrl, CancellationToken cancellationToken)
        {
            serviceUrl =  $"{Constants.BaseUrl}?{serviceUrl}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(serviceUrl)
            };
            return await _httpClientsFactory.SendAsync<WeatherServiceResponse>(request, cancellationToken);
        }

    }
}
