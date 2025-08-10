using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Infrastructure.Hosted;
using Aggregation.Backend.Infrastructure.Options;
using Aggregation.Backend.Infrastructure.Services;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Aggregation.Backend.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(cfg => { cfg.UseInMemoryStorage(); });
            services.AddHangfireServer();


            var allOptions = typeof(NewsOptions).Assembly.GetTypes()
                .Where(s => s.IsAssignableTo(typeof(IHttpClientOptions)))
                .ToList();

            foreach (var type in allOptions)
            {
                IHttpClientOptions options = (IHttpClientOptions)Activator.CreateInstance(type);
                configuration.Bind(type.Name, options);
                var fallbackResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("")
                };

                services.AddHttpClient(type.Name, client =>
                {
                    client.BaseAddress = new Uri(options.BaseUrl);
                    client.DefaultRequestHeaders.Add("user-agent", "AggregationApi/0.1");
                });
                var ifc = typeof(IHttpClientWrapper<>).MakeGenericType(type);
                var impl = typeof(HttpClientWrapper<>).MakeGenericType(type);

                services.Add(new ServiceDescriptor(ifc, impl, ServiceLifetime.Singleton));
            }
            services.AddTransient<IExternalApiService, NewsService>();
            services.AddTransient<IExternalApiService, AirPollutionService>();
            services.AddTransient<IExternalApiService, StockMarketFeedService>();
            services.AddHostedService<StatisticsAnalyzerService>();

            return services;
        }
    }
}