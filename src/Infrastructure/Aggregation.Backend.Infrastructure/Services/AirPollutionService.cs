using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Domain.Dtos.External.Air;
using Aggregation.Backend.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Aggregation.Backend.Infrastructure.Services
{
    public class AirPollutionService(IHttpClientWrapper<AirPollutionOptions> httpClientWrapper, IConfiguration configuration) : IExternalApiService
    {
        private readonly IHttpClientWrapper<AirPollutionOptions> _httpClientWrapper = httpClientWrapper;
        private readonly AirPollutionOptions options = ConfigurationBinder.Get<AirPollutionOptions>(configuration.GetRequiredSection(typeof(AirPollutionOptions).Name));

        public async Task<IList<Dictionary<string, string>>> ListAsync(string category, CancellationToken cancellationToken)
        {
            var queryString = $"state={category}&country=Greece&key={options.ApiKey}";
            var relativePath = string.Join("?", options.ListUri, queryString);
            var response = await _httpClientWrapper.GetAsync(relativePath, cancellationToken);

            if (string.IsNullOrEmpty(response))
            {
                return [];
            }

            var cities = JsonConvert.DeserializeObject<CitiesDto>(response);

            var json = JsonConvert.SerializeObject(cities?.Cities ?? new List<NameOnlyDto>());

            var tasks = new List<Task<Dictionary<string, string>>>();
            foreach (var item in cities?.Cities ?? Enumerable.Empty<NameOnlyDto>())
            {
                var cityName = item.City;
                tasks.Add(Task.Run(async () =>
                {
                    var queryStringPerCity = $"city={cityName}&state={category}&country=Greece&key={options.ApiKey}";
                    var relativePathPerCity = string.Join("?", options.GetUri, queryStringPerCity);

                    var cityResponse = await _httpClientWrapper.GetAsync(relativePathPerCity, cancellationToken);
                    if (string.IsNullOrEmpty(cityResponse))
                    {
                        return new Dictionary<string, string>();
                    }
                    var city = JsonConvert.DeserializeObject<CityDto>(cityResponse);
                    var result = city?.Data?.ToMap() ?? new Dictionary<string, string>();
                    return result;
                }));
            }

            var results = await Task.WhenAll(tasks);
            return results.Where(s=>s.Count>0).ToList();
        }
    }
}