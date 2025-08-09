using System.Net;

namespace Aggregation.Backend.Domain.Exceptions
{
    public class ExternalApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ExternalApiException(string? message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}