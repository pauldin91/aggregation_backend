using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Domain.Dtos.External.StockMarket;
using Aggregation.Backend.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json;

namespace Aggregation.Backend.Infrastructure.Services
{
    public class StockMarketFeedService(IHttpClientWrapper<StockMarketFeedOptions> httpClientWrapper, IConfiguration configuration) : IExternalApiService
    {
        private readonly IHttpClientWrapper<StockMarketFeedOptions> _httpClientWrapper = httpClientWrapper;
        private readonly StockMarketFeedOptions options = ConfigurationBinder.Get<StockMarketFeedOptions>(configuration.GetRequiredSection(typeof(StockMarketFeedOptions).Name));

        public async Task<IList<Dictionary<string, string>>> ListAsync(string category, CancellationToken cancellationToken)
        {
            var queryString = $"function=NEWS_SENTIMENT&tickers={category}&apikey={options.ApiKey}";
            var relativePath = string.Join("?", options.ListUri, queryString);
            var response = await _httpClientWrapper.GetAsync(relativePath, cancellationToken);
            if (string.IsNullOrEmpty(response))
            {
                return new List<Dictionary<string, string>>();
            } else if (JsonConvert.DeserializeObject<InfoDto>(response) is not null) { 
                return new List<Dictionary<string, string>>();

            }
            


            var stockFeed = JsonConvert.DeserializeObject<StockMarketFeedDto>(response);

            var result = stockFeed?.Feed.Select(s=>s.ToMap()).ToList() ?? new List<Dictionary<string,string>>();

            return result;
        }
    }
}