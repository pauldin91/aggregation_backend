using Aggregation.Backend.Infrastructure.Cache;
using Aggregation.Backend.Infrastructure.Options;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aggregation.Backend.Infrastructure.Hosted
{
    public class StatisticsAnalyzerService(ILogger<StatisticsAnalyzerService> logger, IOptions<StatisticsAnalyzerServiceOptions> options, ExternalApiRequestTimingCache externalApiRequestTimingCache) : IHostedService
    {
        private readonly ILogger<StatisticsAnalyzerService> _logger = logger;
        private readonly IOptions<StatisticsAnalyzerServiceOptions> _options = options;
        private readonly ExternalApiRequestTimingCache _externalApiRequestTimingCache = externalApiRequestTimingCache;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            RecurringJob.AddOrUpdate(nameof(StatisticsAnalyzerService), () => CollectPerformanceData(), _options.Value.CronExpression, new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

            return Task.CompletedTask;
        }

        public void CollectPerformanceData()
        {
            try
            {
                var responseTimes = _externalApiRequestTimingCache.GetResponseTimes();
                foreach (var responseTime in responseTimes)
                {
                    var avgPerformance = PerformanceStatisticsCache.AverageResponseTime;
                    if (responseTime.Value > 1.5 * avgPerformance)
                    {
                        _logger.LogWarning("Detected degraded performance of endpoint {Endpoint} with average reponse time  of {EndpointResponseTime} over the last 5 minutes, while average performance is {AvgReponseTime}",
                            responseTime.Key, responseTime.Value, avgPerformance);
                    }
                }
            }
            catch
            {
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            RecurringJob.RemoveIfExists(nameof(StatisticsAnalyzerService));
            return Task.CompletedTask;
        }
    }
}