using Aggregation.Backend.Domain.Constants;
using Aggregation.Backend.Infrastructure.Cache;

namespace Agregation.Backend.Infrastructure.Tests.Cache
{
    public class PerformanceStatisticsCacheTests
    {
        private PerformanceStatisticsCache _cache = null!;

        public PerformanceStatisticsCacheTests()
        {
            _cache = new PerformanceStatisticsCache();
            _cache.Record(50);
            _cache.Record(150);
            _cache.Record(250);
        }

        [Test]
        public void Record_AddsFastRequest_UpdatesStatsCorrectly()
        {
            var stats = _cache.GetStats();

            Assert.That(stats.TotalRequests, Is.EqualTo(3));
            Assert.That(stats.AverageResponseTimeMs, Is.EqualTo((50 + 150 + 250) / 3));
            Assert.That(stats.Buckets[Buckets.Fast], Is.EqualTo(1));
        }

        [Test]
        public void Record_AddsAverageAndSlowRequests_UpdatesBucketsCorrectly()
        {
            var stats = _cache.GetStats();

            Assert.That(stats.Buckets[Buckets.Average], Is.EqualTo(1));
            Assert.That(stats.Buckets[Buckets.Slow], Is.EqualTo(1));
        }
    }
}