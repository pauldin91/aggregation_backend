using Hangfire;
using Microsoft.Extensions.Hosting;

namespace Aggregation.Backend.Infrastructure.Hosted
{
    public class StatisticsAnalyzerService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            RecurringJob.AddOrUpdate(nameof(StatisticsAnalyzerService), () => CollectPerformanceData(), "* * * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
            return Task.CompletedTask;
        }

        public void CollectPerformanceData()
        {
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}