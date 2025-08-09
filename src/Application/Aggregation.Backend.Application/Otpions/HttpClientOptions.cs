using Aggregation.Backend.Application.Interfaces;

namespace Aggregation.Backend.Application.Otpions
{
    public class HttpClientOptions : IHttpClientOptions
    {
        public string BaseUrl { get;  set;  }
        public string ListUri { get;  set;  }
        public string ApiKey { get; set; }
    }
}