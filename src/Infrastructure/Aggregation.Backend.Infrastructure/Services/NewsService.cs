using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Domain.Dtos.External.News;
using Aggregation.Backend.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Aggregation.Backend.Infrastructure.Services
{
    public class NewsService(IHttpClientWrapper<NewsOptions> httpClientWrapper, IConfiguration configuration) : IExternalApiService
    {
        private readonly IHttpClientWrapper<NewsOptions> _httpClientWrapper = httpClientWrapper;
        private readonly NewsOptions options = ConfigurationBinder.Get<NewsOptions>(configuration.GetRequiredSection(typeof(NewsOptions).Name));

        public async Task<IList<Dictionary<string, string>>> ListAsync(string category, CancellationToken cancellationToken)
        {
            var queryString = $"q={category}&page=1&pageSize=100&apiKey={options.ApiKey}";
            var relativePath = string.Join("?", options.ListUri, queryString);
            var response = await _httpClientWrapper.GetAsync(relativePath, cancellationToken);
            if (string.IsNullOrEmpty(response))
            {
                return [];
            }

            var news = JsonConvert.DeserializeObject<ArticlesDto>(response);
            var result = news?.Articles?.Select(s => s.ToMap()).ToList() ?? new List<Dictionary<string,string>>();

            return result;
        }
    }
}