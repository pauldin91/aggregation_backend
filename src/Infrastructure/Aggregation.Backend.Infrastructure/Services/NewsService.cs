using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Domain.Dtos.External.News;
using Aggregation.Backend.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Aggregation.Backend.Infrastructure.Services
{
    public class NewsService(IHttpClientWrapper<NewsOptions> httpClientWrapper, IOptions<NewsOptions> options) : IExternalApiService
    {
        private readonly IHttpClientWrapper<NewsOptions> _httpClientWrapper = httpClientWrapper;

        public async Task<IList<Dictionary<string, string>>> ListAsync(string category, CancellationToken cancellationToken)
        {
            var queryString = $"q={category}&page=1&pageSize=100&apiKey={options.Value.ApiKey}";
            var relativePath = string.Join("?", options.Value.ListUri, queryString);
            var response = await _httpClientWrapper.GetAsync(relativePath, cancellationToken);
            if (string.IsNullOrEmpty(response))
            {
                return [];
            }

            var news = JsonConvert.DeserializeObject<ArticlesDto>(response);
            var result = news?.Articles?.Select(s => s.ToMap()).ToList() ?? new List<Dictionary<string, string>>();

            return result;
        }
    }
}