using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Domain;

namespace WeatherForecast.Infrastructure
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddRepositories()
                .AddExternalHttpServices()
                .AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IWeatherService, WeatherService>();
        }


        private static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddDbContext<WeatherForecastDbContext>(options => options.UseInMemoryDatabase("WeatherForecastDb"))
                .AddScoped<IHttpClientsFactory, HttpClientsFactory>()
                .AddTransient<IWeatherService, WeatherService>()
                .AddScoped<DbContext, WeatherForecastDbContext>()
                .AddScoped<IBaseRepository<WeatherForecastDataModel>, BaseRepository<WeatherForecastDataModel>>()
                .AddScoped<IWeatherForcastRepository, WeatherForcastRepository>()
                .AddScoped<IWeatherServiceRepository, WeatherServiceRepository>();
        }

        private static IServiceCollection AddExternalHttpServices(this IServiceCollection serviceCollection) 
        {
            return serviceCollection
                .AddHttpClient();
        }
    }
}
