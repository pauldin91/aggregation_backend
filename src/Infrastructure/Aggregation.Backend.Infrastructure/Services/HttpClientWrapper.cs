using Aggregation.Backend.Application.Interfaces;
using Polly;
using Polly.Wrap;
using System.Net;

namespace Aggregation.Backend.Infrastructure.Services
{
    public class HttpClientWrapper<T>(IHttpClientFactory httpClientFactory) : IHttpClientWrapper<T>
        where T : IHttpClientOptions, new()
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(typeof(T).Name);
        private static AsyncPolicy<HttpResponseMessage> combinedPolicy = FallbackPolicy();

        public async Task<string> GetAsync(string uri, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await combinedPolicy.ExecuteAsync(() => _httpClient.SendAsync(request));

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return content;
            }
            return string.Empty;
        }

        public static AsyncPolicy<HttpResponseMessage> FallbackPolicy()
        {
            return Policy<HttpResponseMessage>
                    .Handle<HttpRequestException>()
                    .OrResult(r => !r.IsSuccessStatusCode)
                    .FallbackAsync(
                            new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new StringContent("")
                            }
                    );

        }
    }
}