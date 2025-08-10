using Aggregation.Backend.Infrastructure.Cache;
using System.Diagnostics;

namespace Aggregation.Backend.WebApi.Middlewares
{
    public class RequestAnalyticsMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestAnalyticsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();
            if (context.Request.Path.ToString().Contains("aggregates"))
                PerformanceStatisticsCache.Record(stopwatch.ElapsedMilliseconds);
        }
    }
}