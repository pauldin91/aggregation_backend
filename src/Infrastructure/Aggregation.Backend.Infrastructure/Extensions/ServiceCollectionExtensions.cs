using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Infrastructure.Options;
using Aggregation.Backend.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Aggregation.Backend.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            var allOptions = typeof(NewsOptions).Assembly.GetTypes()
                .Where(s => s.IsAssignableTo(typeof(IHttpClientOptions)) )
                .ToList();

            foreach (var type in allOptions)
            {
                IHttpClientOptions options = (IHttpClientOptions)Activator.CreateInstance(type);
                configuration.Bind(type.Name, options);
                services.AddHttpClient(type.Name, client =>
                {
                    client.BaseAddress = new Uri(options.BaseUrl);
                    client.DefaultRequestHeaders.Add("user-agent", "Aggregate-Api/0.1");
                    client.DefaultRequestHeaders.Add("x-api-key", options.ApiKey);
                });
                var ifc = typeof(IHttpClientWrapper<>).MakeGenericType(type);
                var impl = typeof(HttpClientWrapper<>).MakeGenericType(type);

                services.Add(new ServiceDescriptor(ifc, impl, ServiceLifetime.Singleton));
            }
            services.AddTransient<IExternalApiService, NewsService>();
            services.AddTransient<IExternalApiService, MusicService>();
            services.AddTransient<IExternalApiService, WeatherService>();

            return services;
        }
    }
}