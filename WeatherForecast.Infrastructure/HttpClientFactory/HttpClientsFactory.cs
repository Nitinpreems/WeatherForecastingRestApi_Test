
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WeatherForecast.Infrastructure
{
    public class HttpClientsFactory : IHttpClientsFactory
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientsFactory> _logger;
        public HttpClientsFactory(IHttpClientFactory httpClientFactory, ILogger<HttpClientsFactory> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task<T> SendAsync<T>(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            try
            {
                using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
                response.EnsureSuccessStatusCode();

                return await ParseResponse<T>(response, cancellationToken);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken)
        {
            try
            {
                using var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                return await ParseResponse<T>(response, cancellationToken);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<T> ParseResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {

            var resultContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(resultContent); ;
        }
    }
}
