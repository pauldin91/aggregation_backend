using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Domain.Dtos.Aggregates;
using Aggregation.Backend.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using NewsAPI.Models;
using Newtonsoft.Json;

namespace Aggregation.Backend.Infrastructure.Services
{
    public class NewsService(IHttpClientWrapper<NewsOptions> httpClientWrapper, IConfiguration configuration) : IExternalApiService
    {
        private readonly IHttpClientWrapper<NewsOptions> _httpClientWrapper = httpClientWrapper;
        private readonly NewsOptions options = ConfigurationBinder.Get<NewsOptions>(configuration.GetRequiredSection(typeof(NewsOptions).Name));

        public async Task<IList<AggregateResponse>> ListAsync(string category, CancellationToken cancellationToken)
        {
            var queryString = $"q={category}&page=1&pageSize=100";
            var relativePath = string.Join("?", options.ListUri, queryString);
            var response = await _httpClientWrapper.GetAsync(relativePath, cancellationToken);
            var result = JsonConvert.DeserializeObject<ArticlesResult>(response);

            return result.Articles.Select(s => new AggregateResponse(s.Author,s.Title, s.Content,s.PublishedAt)).ToList();
        }
    }
}