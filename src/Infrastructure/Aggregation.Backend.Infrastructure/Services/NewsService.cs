using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Aggregation.Backend.Infrastructure.Services
{
    public class NewsService(IHttpClientWrapper<NewsOptions> httpClientWrapper, IConfiguration configuration) : IExternalApiService
    {
        private readonly IHttpClientWrapper<NewsOptions> _httpClientWrapper = httpClientWrapper;
        private readonly NewsOptions options = ConfigurationBinder.Get<NewsOptions>(configuration.GetRequiredSection(typeof(NewsOptions).Name));

        public async Task<IList<Dictionary<string, string>>> ListAsync(string category, CancellationToken cancellationToken)
        {
            var queryString = $"q={category}&page=1&pageSize=100";
            var relativePath = string.Join("?", options.ListUri, queryString);
            var response = await _httpClientWrapper.GetAsync(relativePath, cancellationToken);

            var result = ParseFromJson(response);

            return result;
        }

        public static List<Dictionary<string, string>> ParseFromJson(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement itemsElement = doc.RootElement.GetProperty("articles");

            var items = new List<Dictionary<string, string>>();

            foreach (JsonElement item in itemsElement.EnumerateArray())
            {
                var dict = new Dictionary<string, string>();
                foreach (JsonProperty prop in item.EnumerateObject())
                {
                    dict[prop.Name] = prop.Value.ToString();
                }
                items.Add(dict);
            }
            return items;
        }
    }
}