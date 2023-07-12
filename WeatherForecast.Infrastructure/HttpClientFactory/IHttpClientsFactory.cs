

namespace WeatherForecast.Infrastructure
{
    public interface IHttpClientsFactory
    {

        Task<T> SendAsync<T>(HttpRequestMessage requestMessage, CancellationToken cancellationToken);
        Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken);

    }
}
