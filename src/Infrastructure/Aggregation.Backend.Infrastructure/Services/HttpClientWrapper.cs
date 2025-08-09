using Aggregation.Backend.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;

namespace Aggregation.Backend.Infrastructure.Services
{
    public class HttpClientWrapper<T>(IHttpClientFactory httpClientFactory,IConfiguration configuration) : IHttpClientWrapper<T>
        where T : IHttpClientOptions, new()
    {

        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(typeof(T).Name);

        public async Task<string> GetAsync(string uri, CancellationToken cancellationToken)
        {
            
            var request = new HttpRequestMessage(HttpMethod.Get,uri);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return content;
        }
    }
}